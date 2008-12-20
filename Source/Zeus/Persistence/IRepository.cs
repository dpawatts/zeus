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
		T Get<T>(TKey id) where T : ContentItem;
		TEntity Load(int id);
		void Save(TEntity toMove);
		void SaveOrUpdate(TEntity contentItem);
	}
}
