﻿using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.ContentTypes.Properties;

namespace Zeus.Persistence
{
	public class ContentPersister : IPersister
	{
		private IRepository<int, ContentItem> _contentRepository;
		private IRepository<int, LinkDetail> _linkRepository;

		public ContentPersister(IRepository<int, ContentItem> contentRepository, IRepository<int, LinkDetail> linkRepository)
		{
			_contentRepository = contentRepository;
			_linkRepository = linkRepository;
		}

		/// <summary>Occurs when an item has been deleted</summary>
		public event EventHandler<ItemEventArgs> ItemDeleted;

		/// <summary>Occurs when an item has been saved</summary>
		public event EventHandler<ItemEventArgs> ItemSaved;

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
			if (source is ISelfPersister)
				return (source as ISelfPersister).CopyTo(destination);

			ContentItem cloned = source.Clone(includeChildren);

			cloned.Parent = destination;
			Save(cloned);

			return cloned;
		}

		public void Delete(ContentItem contentItem)
		{
			if (contentItem is ISelfPersister)
			{
				((ISelfPersister) contentItem).Delete();
			}
			else
			{
				using (ITransaction transaction = _contentRepository.BeginTransaction())
				{
					DeleteRecursive(contentItem);
					transaction.Commit();
				}
			}
			if (ItemDeleted != null)
				ItemDeleted(this, new ItemEventArgs(contentItem));
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
			foreach (LinkDetail detail in _linkRepository.Where(ld => ld.TypedValue == itemNoMore))
			{
				if (detail.EnclosingCollection != null)
					detail.EnclosingCollection.Remove(detail);
				object test = detail.EnclosingItem.Details.Values; // TODO: Investigate why this is necessary, on a PersistentGenericMap
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

		public void Save(ContentItem contentItem)
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
			if (ItemSaved != null)
				ItemSaved(this, new ItemEventArgs(contentItem));
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
	}
}
