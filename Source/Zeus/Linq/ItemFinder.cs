using System;
using System.Linq;
using NHibernate.Linq;
using Zeus.Persistence;

namespace Zeus.Linq
{
	public class ItemFinder : IItemFinder
	{
		private ISessionProvider _sessionProvider;

		public ItemFinder(ISessionProvider sessionProvider)
		{
			_sessionProvider = sessionProvider;
		}

		private IOrderedQueryable<ContentItem> ContentItems
		{
			get { return _sessionProvider.OpenSession.Linq<ContentItem>(); }
		}

		#region IEnumerable<ContentItem> Members

		public System.Collections.Generic.IEnumerator<ContentItem> GetEnumerator()
		{
			return this.ContentItems.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.ContentItems.GetEnumerator();
		}

		#endregion

		#region IQueryable Members

		public Type ElementType
		{
			get { return this.ContentItems.ElementType; }
		}

		public System.Linq.Expressions.Expression Expression
		{
			get { return this.ContentItems.Expression; }
		}

		public IQueryProvider Provider
		{
			get { return this.ContentItems.Provider; }
		}

		#endregion
	}
}