using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zeus.ContentProperties;

namespace Zeus.Persistence
{
	/// <summary>
	/// Handles saving and restoring versions of items.
	/// </summary>
	public class VersionManager : IVersionManager
	{
		#region Fields

		private IRepository<int, ContentItem> itemRepository;
		private readonly IFinder<ContentItem> finder;

		#endregion

		#region Constructor

		public VersionManager(IRepository<int, ContentItem> itemRepository, IFinder<ContentItem> finder)
		{
			this.itemRepository = itemRepository;
			this.finder = finder;
		}

		#endregion

		#region Events

		/// <summary>Occurs before an item is saved</summary>
		public event EventHandler<CancelItemEventArgs> ItemSavingVersion;

		/// <summary>Occurs before an item is saved</summary>
		public event EventHandler<ItemEventArgs> ItemSavedVersion;

		/// <summary>Occurs before an item is saved</summary>
		public event EventHandler<CancelDestinationEventArgs> ItemReplacingVersion;

		/// <summary>Occurs before an item is saved</summary>
		public event EventHandler<ItemEventArgs> ItemReplacedVersion;

		#endregion

		#region Versioning Methods

		/// <summary>Creates an old version of an item. This must be called before the item item is modified.</summary>
		/// <param name="item">The item to create a old version of.</param>
		/// <returns>The old version.</returns>
		public virtual ContentItem SaveVersion(ContentItem item)
		{
			CancelItemEventArgs args = new CancelItemEventArgs(item);
			if (ItemSavingVersion != null)
				ItemSavingVersion.Invoke(this, args);
			if (!args.Cancel)
			{
				item = args.AffectedItem;

				ContentItem oldVersion = item.Clone(false);
				oldVersion.Expires = Utility.CurrentTime().AddSeconds(-1);
				oldVersion.Updated = Utility.CurrentTime().AddSeconds(-1);
				oldVersion.Parent = null;
				oldVersion.TranslationOf = null;
				oldVersion.VersionOf = item;
				if (item.Parent != null)
					oldVersion["ParentID"] = item.Parent.ID;
				if (item.TranslationOf != null)
					oldVersion["TranslationOfID"] = item.TranslationOf.ID;
				itemRepository.SaveOrUpdate(oldVersion);

				if (ItemSavedVersion != null)
					ItemSavedVersion.Invoke(this, new ItemEventArgs(oldVersion));

				return oldVersion;
			}
			return null;
		}

		/// <summary>Update a page version with another, i.e. save a version of the current item and replace it with the replacement item. Returns a version of the previously published item.</summary>
		/// <param name="currentItem">The item that will be stored as a previous version.</param>
		/// <param name="replacementItem">The item that will take the place of the current item using it's ID. Any saved version of this item will not be modified.</param>
		/// <returns>A version of the previously published item.</returns>
		public virtual ContentItem ReplaceVersion(ContentItem currentItem, ContentItem replacementItem)
		{
			CancelDestinationEventArgs args = new CancelDestinationEventArgs(currentItem, replacementItem);
			if (ItemReplacingVersion != null)
				ItemReplacingVersion.Invoke(this, args);
			if (!args.Cancel)
			{
				currentItem = args.AffectedItem;
				replacementItem = args.Destination;

				using (ITransaction transaction = itemRepository.BeginTransaction())
				{
					ContentItem versionOfCurrentItem = SaveVersion(currentItem);
					ClearAllDetails(currentItem);

					UpdateCurrentItemData(currentItem, replacementItem);

					currentItem.Updated = Utility.CurrentTime();
					itemRepository.Update(currentItem);

					if (ItemReplacedVersion != null)
						ItemReplacedVersion.Invoke(this, new ItemEventArgs(replacementItem));
					if (replacementItem.VersionOf == currentItem)
						itemRepository.Delete(replacementItem);

					itemRepository.Flush();
					transaction.Commit();

					return versionOfCurrentItem;
				}
			}
			return currentItem;
		}

		#region ReplaceVersion Helper Methods

		private static void UpdateCurrentItemData(ContentItem currentItem, ContentItem replacementItem)
		{
			for (Type t = currentItem.GetType(); t.BaseType != null; t = t.BaseType)
			{
				foreach (PropertyInfo pi in t.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
					if (pi.GetCustomAttributes(typeof(CopyAttribute), true).Length > 0)
						pi.SetValue(currentItem, pi.GetValue(replacementItem, null), null);

				foreach (FieldInfo fi in t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
				{
					if (fi.GetCustomAttributes(typeof(CopyAttribute), true).Length > 0)
						fi.SetValue(currentItem, fi.GetValue(replacementItem));
					if (fi.Name == "_url")
						fi.SetValue(currentItem, null);
				}
			}

			foreach (PropertyData detail in replacementItem.Details.Values)
				currentItem[detail.Name] = detail.Value;

			foreach (PropertyCollection collection in replacementItem.DetailCollections.Values)
			{
				PropertyCollection newCollection = currentItem.GetDetailCollection(collection.Name, true);
				foreach (PropertyData detail in collection.Details)
					newCollection.Add(detail.Value);
			}
		}

		private void ClearAllDetails(ContentItem item)
		{
			item.Details.Clear();
			foreach (PropertyCollection collection in item.DetailCollections.Values)
				collection.Details.Clear();
			item.DetailCollections.Clear();
		}

		#endregion

		/// <summary>Retrieves all versions of an item including the master version.</summary>
		/// <param name="publishedItem">The item whose versions to get.</param>
		/// <returns>A list of versions of the item.</returns>
		public IList<ContentItem> GetVersionsOf(ContentItem publishedItem)
		{
			if (publishedItem.ID == 0)
				return new List<ContentItem>();

			return GetVersionsQuery(publishedItem)
				.ToList();
		}

		/// <summary>Retrieves all versions of an item including the master version.</summary>
		/// <param name="publishedItem">The item whose versions to get.</param>
		/// <param name="count">The number of versions to get.</param>
		/// <returns>A list of versions of the item.</returns>
		public IList<ContentItem> GetVersionsOf(ContentItem publishedItem, int count)
		{
			return GetVersionsOf(publishedItem)
				.Take(count)
				.ToList();
		}

		private IEnumerable<ContentItem> GetVersionsQuery(ContentItem publishedItem)
		{
			return finder.FindAll<ContentItem>()
				.Where(ci => ci.VersionOf == publishedItem || ci.ID == publishedItem.ID)
				.OrderBy(ci => ci.Version);
		}

		#endregion
	}
}