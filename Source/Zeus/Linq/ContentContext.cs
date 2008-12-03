using System;
using System.Linq;
using NHibernate.Linq;
using Zeus.Persistence;

namespace Zeus.Linq
{
	public class ContentContext
	{
		private ISessionProvider _sessionProvider;

		public ContentContext(ISessionProvider sessionProvider)
		{
			_sessionProvider = sessionProvider;
		}

		public IOrderedQueryable<ContentItem> ContentItems
		{
			get
			{
				return _sessionProvider.OpenSession.Linq<ContentItem>();
			}
		}

		public IOrderedQueryable<T> Elements<T>()
		{
			return _sessionProvider.OpenSession.Linq<T>();
		}
	}
}
