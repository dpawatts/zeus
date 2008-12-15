using System;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Linq.Expressions;
using System.Linq;

namespace Zeus.Persistence
{
	public class Repository<TKey, TEntity> : IRepository<TKey, TEntity>
	{
		private ISessionProvider _sessionProvider;

		public Repository(ISessionProvider sessionProvider)
		{
			_sessionProvider = sessionProvider;
		}

		public ITransaction BeginTransaction()
		{
			return _sessionProvider.OpenSession.BeginTransaction();
		}

		public void Delete(TEntity contentItem)
		{
			_sessionProvider.OpenSession.Delete(contentItem);
		}

		public TEntity Get(TKey id)
		{
			return _sessionProvider.OpenSession.Get<TEntity>(id);
		}

		public TEntity Load(int id)
		{
			return _sessionProvider.OpenSession.Load<TEntity>(id);
		}

		public void Save(TEntity contentItem)
		{
			_sessionProvider.OpenSession.Save(contentItem);
		}

		public void SaveOrUpdate(TEntity contentItem)
		{
			_sessionProvider.OpenSession.SaveOrUpdate(contentItem);
		}

		#region IEnumerable<TEntity> Members

		public IEnumerator<TEntity> GetEnumerator()
		{
			return _sessionProvider.OpenSession.Linq<TEntity>().GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _sessionProvider.OpenSession.Linq<TEntity>().GetEnumerator();
		}

		#endregion

		#region IQueryable Members

		public Type ElementType
		{
			get { return _sessionProvider.OpenSession.Linq<TEntity>().ElementType; }
		}

		public Expression Expression
		{
			get { return _sessionProvider.OpenSession.Linq<TEntity>().Expression; }
		}

		public IQueryProvider Provider
		{
			get { return _sessionProvider.OpenSession.Linq<TEntity>().Provider; }
		}

		#endregion
	}
}
