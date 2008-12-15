using System.Reflection;
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

		public void Delete(ContentItem contentItem)
		{
			using (ITransaction transaction = _contentRepository.BeginTransaction())
			{
				DeleteRecursive(contentItem);
				transaction.Commit();
			}
		}

		private void DeleteRecursive(ContentItem contentItem)
		{
			foreach (ContentItem child in contentItem.Children)
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
				detail.EnclosingItem.Details.Remove(detail.Name);
				_linkRepository.Delete(detail);
			}
		}

		public ContentItem Get(int id)
		{
			return _contentRepository.Get(id);
		}

		public ContentItem Load(int id)
		{
			return _contentRepository.Load(id);
		}

		public void Move(ContentItem toMove, ContentItem newParent)
		{
			using (ITransaction transaction = _contentRepository.BeginTransaction())
			{
				toMove.AddTo(newParent);
				_contentRepository.Save(toMove);
				transaction.Commit();
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
			contentItem.Updated = DateTime.Now;
			using (ITransaction transaction = _contentRepository.BeginTransaction())
			{
				_contentRepository.SaveOrUpdate(contentItem);
				contentItem.AddTo(contentItem.Parent);
				transaction.Commit();
			}
		}
	}
}
