namespace Zeus.Persistence.NH
{
	public class Repository<TKey, TEntity> : IRepository<TKey, TEntity>
	{
		private readonly ISessionProvider _sessionProvider;

		public Repository(ISessionProvider sessionProvider)
		{
			_sessionProvider = sessionProvider;
		}

		public ITransaction BeginTransaction()
		{
			return new Transaction(_sessionProvider);
		}

		public void Flush()
		{
			_sessionProvider.OpenSession.Session.Flush();
		}

		public void Delete(TEntity contentItem)
		{
			_sessionProvider.OpenSession.Session.Delete(contentItem);
		}

		public TEntity Get(TKey id)
		{
			return _sessionProvider.OpenSession.Session.Get<TEntity>(id);
		}

		public T Get<T>(TKey id)
			where T : TEntity
		{
			return _sessionProvider.OpenSession.Session.Get<T>(id);
		}

		public TEntity Load(TKey id)
		{
			return _sessionProvider.OpenSession.Session.Load<TEntity>(id);
		}

		public void Save(TEntity contentItem)
		{
			_sessionProvider.OpenSession.Session.Save(contentItem);
		}

		public void SaveOrUpdate(TEntity contentItem)
		{
			_sessionProvider.OpenSession.Session.SaveOrUpdate(contentItem);
		}

		public void Update(TEntity entity)
		{
			_sessionProvider.OpenSession.Session.Update(entity);
		}
	}
}