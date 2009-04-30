using NHibernate;
using Zeus.Web;

namespace Zeus.Persistence.NH
{
	public class SessionProvider : ISessionProvider
	{
		private const string RequestItemsKey = "SessionProvider.Session";
		private readonly ISessionFactory _sessionFactory;
		private readonly INotifyingInterceptor _interceptor;
		private readonly IWebContext _webContext;

		public SessionProvider(IConfigurationBuilder configurationBuilder, INotifyingInterceptor interceptor, IWebContext webContext)
		{
			_sessionFactory = configurationBuilder.Configuration.BuildSessionFactory();
			_interceptor = interceptor;
			_webContext = webContext;
		}

		private SessionContext CurrentSession
		{
			get { return _webContext.RequestItems[RequestItemsKey] as SessionContext; }
			set { _webContext.RequestItems[RequestItemsKey] = value; }
		}

		public virtual SessionContext OpenSession
		{
			get
			{
				SessionContext sc = CurrentSession;
				if (sc == null)
				{
					ISession s = _sessionFactory.OpenSession(_interceptor);
					s.FlushMode = FlushMode.Commit;
					CurrentSession = sc = new SessionContext(this, s);
				}
				return sc;
			}
		}

		public void Dispose()
		{
			SessionContext sc = CurrentSession;

			if (sc != null)
			{
				sc.Session.Dispose();
				CurrentSession = null;
			}
		}
	}
}
