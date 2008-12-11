using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zeus.Persistence
{
	public class ContentPersister : IPersister
	{
		private ISessionProvider _sessionProvider;

		public ContentPersister(ISessionProvider sessionProvider)
		{
			_sessionProvider = sessionProvider;
		}

		public void Delete(ContentItem contentItem)
		{
			using (ITransaction transaction = _sessionProvider.OpenSession.BeginTransaction())
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

			// delete inbound links

			_sessionProvider.OpenSession.Delete(contentItem);
		}

		public ContentItem Get(int id)
		{
			return _sessionProvider.OpenSession.Get<ContentItem>(id);
		}

		public ContentItem Load(int id)
		{
			return _sessionProvider.OpenSession.Load<ContentItem>(id);
		}

		public void Move(ContentItem toMove, ContentItem newParent)
		{
			using (ITransaction transaction = _sessionProvider.OpenSession.BeginTransaction())
			{
				toMove.AddTo(newParent);
				Save(toMove);
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
			using (ITransaction transaction = _sessionProvider.OpenSession.BeginTransaction())
			{
				_sessionProvider.OpenSession.SaveOrUpdate(contentItem);
				contentItem.AddTo(contentItem.Parent);
				transaction.Commit();
			}
		}
	}
}
