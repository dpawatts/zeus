using System;
using Zeus.ContentTypes;
using Zeus.Persistence;
using Zeus.Security;
using Zeus.Web;

namespace Zeus.Admin.RecycleBin
{
	/// <summary>
	/// Can throw and restore items. Thrown items are moved to a trash 
	/// container item.
	/// </summary>
	public class RecycleBinHandler : IRecycleBinHandler
	{
		public const string FormerName = "FormerName";
		public const string FormerParent = "FormerParent";
		public const string FormerExpires = "FormerExpires";
		public const string DeletedDate = "DeletedDate";
		private readonly IPersister _persister;
		private readonly IContentTypeManager _contentTypeManager;
		private readonly IHost _host;

		public RecycleBinHandler(IPersister persister, IContentTypeManager definitions, IHost host)
		{
			_persister = persister;
			_contentTypeManager = definitions;
			_host = host;
		}

		/// <summary>Occurs before an item is thrown.</summary>
		public event EventHandler<CancelItemEventArgs> ItemThrowing;

		/// <summary>Occurs after an item has been thrown.</summary>
		public event EventHandler<ItemEventArgs> ItemThrowed;

		/// <summary>The container of thrown items.</summary>
		IRecycleBin IRecycleBinHandler.TrashContainer
		{
			get { return GetTrashContainer(true); }
		}

		public RecycleBinContainer GetTrashContainer(bool create)
		{
			ContentItem rootItem = _persister.Get(_host.CurrentSite.RootItemID);
			RecycleBinContainer trashContainer = rootItem.GetChild("RecycleBin") as RecycleBinContainer;
			if (create && trashContainer == null)
			{
				trashContainer = _contentTypeManager.CreateInstance<RecycleBinContainer>(rootItem);
				trashContainer.Name = "RecycleBin";
				trashContainer.Title = "Recycle Bin";
				trashContainer.Visible = false;
				//trashContainer.AuthorizationRules.Add(new AuthorizationRule(trashContainer, "admin"));
				//trashContainer.AuthorizationRules.Add(new AuthorizationRule(trashContainer, "Editors"));
				//trashContainer.AuthorizationRules.Add(new AuthorizationRule(trashContainer, "Administrators"));
				trashContainer.SortOrder = int.MaxValue - 1000000;
				_persister.Save(trashContainer);
			}
			return trashContainer;
		}

		/// <summary>Checks if the trash is enabled, the item is not already thrown and for the NotThrowable attribute.</summary>
		/// <param name="affectedItem">The item to check.</param>
		/// <returns>True if the item may be thrown.</returns>
		public bool CanThrow(ContentItem affectedItem)
		{
			RecycleBinContainer trash = GetTrashContainer(false);
			bool enabled = trash == null || trash.Enabled;
			bool alreadyThrown = IsInTrash(affectedItem);
			bool throwable = affectedItem.GetType().GetCustomAttributes(typeof(NotThrowableAttribute), true).Length == 0;
			return enabled && !alreadyThrown && throwable;
		}

		/// <summary>Throws an item in a way that it later may be restored to it's original location at a later stage.</summary>
		/// <param name="item">The item to throw.</param>
		public virtual void Throw(ContentItem item)
		{
			CancelItemEventArgs args = Invoke(ItemThrowing, new CancelItemEventArgs(item));
			if (!args.Cancel)
			{
				item = args.AffectedItem;

				ExpireTrashedItem(item);
				item.AddTo(GetTrashContainer(true));

				try
				{
					_persister.Save(item);
				}
				catch (PermissionDeniedException ex)
				{
					throw new PermissionDeniedException("Permission denied while moving item to trash. Try disabling security checks using Zeus.Context.Security or preventing items from being moved to the trash with the [NonThrowable] attribute", ex);
				}

				Invoke(ItemThrowed, new ItemEventArgs(item));
			}
		}

		/// <summary>Expires an item that has been thrown so that it's not accessible to external users.</summary>
		/// <param name="item">The item to restore.</param>
		public virtual void ExpireTrashedItem(ContentItem item)
		{
			item[FormerName] = item.Name;
			item[FormerParent] = item.Parent;
			item[FormerExpires] = item.Expires;
			item[DeletedDate] = DateTime.Now;
			item.Expires = DateTime.Now;
			item.Name = item.ID.ToString();

			foreach (ContentItem child in item.Children)
				ExpireTrashedItem(child);
		}

		/// <summary>Restores an item to the original location.</summary>
		/// <param name="item">The item to restore.</param>
		public virtual void Restore(ContentItem item)
		{
			ContentItem parent = (ContentItem) item["FormerParent"];
			RestoreValues(item);
			_persister.Save(item);
			_persister.Move(item, parent);
		}

		/// <summary>Removes expiry date and metadata set during throwing.</summary>
		/// <param name="item">The item to reset.</param>
		public virtual void RestoreValues(ContentItem item)
		{
			item.Name = (string) item["FormerName"];
			item.Expires = (DateTime?) item["FormerExpires"];

			item["FormerName"] = null;
			item["FormerParent"] = null;
			item["FormerExpires"] = null;
			item["DeletedDate"] = null;

			foreach (ContentItem child in item.Children)
				RestoreValues(child);
		}

		/// <summary>Determines wether an item has been thrown away.</summary>
		/// <param name="item">The item to check.</param>
		/// <returns>True if the item is in the scraps.</returns>
		public bool IsInTrash(ContentItem item)
		{
			RecycleBinContainer trash = GetTrashContainer(false);
			return trash != null && Find.IsDescendantOrSelf(item, trash);
		}

		protected virtual T Invoke<T>(EventHandler<T> handler, T args)
				where T : ItemEventArgs
		{
			if (handler != null && args.AffectedItem.VersionOf == null)
				handler.Invoke(this, args);
			return args;
		}
	}
}