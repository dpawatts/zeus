using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.ContentProperties;
using Zeus.Persistence.Specifications;

namespace Zeus.Persistence
{
	public class ContentPersister : IPersister
	{
		#region Fields

		private readonly IRepository<int, ContentItem> _contentRepository;
		private readonly IRepository<int, LinkProperty> _linkRepository;
		private readonly IFinder<LinkProperty> _linkFinder;

		#endregion

		#region Constructor

		public ContentPersister(IRepository<int, ContentItem> contentRepository, IRepository<int, LinkProperty> linkRepository, IFinder<LinkProperty> linkFinder)
		{
			_contentRepository = contentRepository;
			_linkRepository = linkRepository;
			_linkFinder = linkFinder;
		}

		#endregion

		#region Properties

		public IRepository<int, ContentItem> Repository
		{
			get { return _contentRepository; }
		}

		#endregion

		#region Events

		/// <summary>Occurs before an item is saved</summary>
		public event EventHandler<CancelItemEventArgs> ItemSaving;

		/// <summary>Occurs when an item has been saved</summary>
		public event EventHandler<ItemEventArgs> ItemSaved;

		/// <summary>Occurs before an item is deleted</summary>
		public event EventHandler<CancelItemEventArgs> ItemDeleting;

		/// <summary>Occurs when an item has been deleted</summary>
		public event EventHandler<ItemEventArgs> ItemDeleted;

		/// <summary>Occurs before an item is moved</summary>
		public event EventHandler<CancelDestinationEventArgs> ItemMoving;

		/// <summary>Occurs when an item has been moved</summary>
		public event EventHandler<DestinationEventArgs> ItemMoved;

		/// <summary>Occurs before an item is copied</summary>
		public event EventHandler<CancelDestinationEventArgs> ItemCopying;

		/// <summary>Occurs when an item has been copied</summary>
		public event EventHandler<DestinationEventArgs> ItemCopied;

		/// <summary>Occurs when an item is loaded</summary>
		public event EventHandler<ItemEventArgs> ItemLoaded;

		#endregion

		#region Methods

		/// <summary>Copies an item and all sub-items to a destination</summary>
		/// <param name="source">The item to copy</param>
		/// <param name="destination">The destination below which to place the copied item</param>
		/// <returns>The copied item</returns>
		public virtual ContentItem Copy(ContentItem source, ContentItem destination)
		{
			return Copy(source, destination, true);
		}

		/// <summary>Copies an item and all sub-items to a destination</summary>
		/// <param name="source">The item to copy</param>
		/// <param name="destination">The destination below which to place the copied item</param>
		/// <param name="includeChildren">Whether child items should be copied as well.</param>
		/// <returns>The copied item</returns>
		public virtual ContentItem Copy(ContentItem source, ContentItem destination, bool includeChildren)
		{
			return Utility.InvokeEvent(ItemCopying, this, source, destination, (copiedItem, destinationItem) =>
			{
				if (source is ISelfPersister)
					return (source as ISelfPersister).CopyTo(destination);

				ContentItem cloned = source.Clone(includeChildren);

				cloned.Parent = destination;
				Save(cloned);

				Invoke(ItemCopied, new DestinationEventArgs(cloned, destinationItem));

				return cloned;
			});
		}

		public void Delete(ContentItem itemNoMore)
		{
			Utility.InvokeEvent(ItemDeleting, itemNoMore, this, DeleteAction);
		}

		private void DeleteAction(ContentItem itemNoMore)
		{
			if (itemNoMore is ISelfPersister)
			{
				((ISelfPersister) itemNoMore).Delete();
			}
			else
			{
				using (ITransaction transaction = _contentRepository.BeginTransaction())
				{
					DeleteRecursive(itemNoMore);
					transaction.Commit();
				}
			}
			Invoke(ItemDeleted, new ItemEventArgs(itemNoMore));
		}

		private void DeleteRecursive(ContentItem contentItem)
		{
			List<ContentItem> children = new List<ContentItem>(contentItem.Children);
			foreach (ContentItem child in children)
				DeleteRecursive(child);

			contentItem.AddTo(null);

			DeleteInboundLinks(contentItem);

			_contentRepository.Delete(contentItem);
		}

		private void DeleteInboundLinks(ContentItem itemNoMore)
		{
			foreach (LinkProperty detail in _linkFinder.FindBySpecification(new Specification<LinkProperty>(ld => ld.LinkedItem == itemNoMore)))
			{
				if (detail.EnclosingCollection != null)
					detail.EnclosingCollection.Remove(detail);
				object test = detail.EnclosingItem.Details; // TODO: Investigate why this is necessary, on a PersistentGenericMap
				detail.EnclosingItem.Details.Remove(detail.Name);
				_linkRepository.Delete(detail);
			}
		}

		public ContentItem Get(int id)
		{
			return _contentRepository.Get(id);
		}

		public T Get<T>(int id)
			where T : ContentItem
		{
			return _contentRepository.Get<T>(id);
		}

		public ContentItem Load(int id)
		{
			return _contentRepository.Load(id);
		}

		public void Move(ContentItem toMove, ContentItem newParent)
		{
			Utility.InvokeEvent(ItemMoving, this, toMove, newParent, MoveAction);
		}

		private ContentItem MoveAction(ContentItem toMove, ContentItem newParent)
		{
			if (toMove is ISelfPersister)
			{
				((ISelfPersister) toMove).MoveTo(newParent);
			}
			else
			{
				using (ITransaction transaction = _contentRepository.BeginTransaction())
				{
					toMove.AddTo(newParent);
					_contentRepository.Save(toMove);
					transaction.Commit();
				}
			}
			Invoke(ItemMoved, new DestinationEventArgs(toMove, newParent));
			return null;
		}

		public void UpdateSortOrder(ContentItem contentItem, int newPos)
		{
			IEnumerable<ContentItem> siblings = contentItem.Parent.Children.Where(c => c != contentItem).OrderBy(c => c.SortOrder);
			IEnumerable<ContentItem> previousSiblings = siblings.Where(c => c.SortOrder <= newPos).OrderBy(c => c.SortOrder);
			IEnumerable<ContentItem> nextSiblings = siblings.Where(c => c.SortOrder >= newPos).OrderBy(c => c.SortOrder);

			int currentSortOrder = 0;
			foreach (ContentItem sibling in previousSiblings)
				sibling.SortOrder = currentSortOrder++;
			contentItem.SortOrder = currentSortOrder++;
			foreach (ContentItem sibling in nextSiblings)
				sibling.SortOrder = currentSortOrder++;

			foreach (ContentItem item in siblings)
				Save(item);
		}

		public void Save(ContentItem unsavedItem)
		{
			Utility.InvokeEvent(ItemSaving, unsavedItem, this, SaveAction);
		}

		private void SaveAction(ContentItem contentItem)
		{
			if (contentItem is ISelfPersister)
			{
				((ISelfPersister) contentItem).Save();
			}
			else
			{
				contentItem.Updated = DateTime.Now;
				using (ITransaction transaction = _contentRepository.BeginTransaction())
				{
					_contentRepository.SaveOrUpdate(contentItem);
					contentItem.AddTo(contentItem.Parent);
					EnsureSortOrder(contentItem);
					transaction.Commit();
				}
			}
			Invoke(ItemSaved, new ItemEventArgs(contentItem));
		}

		private void EnsureSortOrder(ContentItem unsavedItem)
		{
			if (unsavedItem.Parent != null)
			{
				IEnumerable<ContentItem> updatedItems = Utility.UpdateSortOrder(unsavedItem.Parent.Children);
				foreach (ContentItem updatedItem in updatedItems)
					_contentRepository.SaveOrUpdate(updatedItem);
			}
		}

		protected virtual T Invoke<T>(EventHandler<T> handler, T args)
						where T : ItemEventArgs
		{
			if (handler != null && args.AffectedItem.VersionOf == null)
				handler.Invoke(this, args);
			return args;
		}

		#endregion
	}
}
