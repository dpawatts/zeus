using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using System.Reflection;
using System.Web;

namespace Zeus.Persistence
{
	public class SessionProvider : ISessionProvider
	{
		private ISessionFactory _sessionFactory;
		private IInterceptor _interceptor;

		public ISession OpenSession
		{
			get
			{
				ISession session = HttpContext.Current.Items["OpenSession"] as ISession;
				if (session == null)
					HttpContext.Current.Items["OpenSession"] = session = _sessionFactory.OpenSession(_interceptor);
				return session;
			}
		}

		public SessionProvider(IConfigurationBuilder configurationBuilder, IInterceptor interceptor)
		{
			_sessionFactory = configurationBuilder.Configuration.BuildSessionFactory();
			_interceptor = interceptor;
		}
	}
}
