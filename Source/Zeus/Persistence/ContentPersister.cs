using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using System;

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
