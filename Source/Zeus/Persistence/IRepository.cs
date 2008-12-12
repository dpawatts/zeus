using System;
using NHibernate;
using System.Linq;

namespace Zeus.Persistence
{
	public interface IRepository<TKey, TEntity> : IQueryable<TEntity>
	{
		ITransaction BeginTransaction();
		void Delete(TEntity contentItem);
		TEntity Get(TKey id);
		TEntity Load(int id);
		void SaveOrUpdate(TEntity contentItem);
	}
}
