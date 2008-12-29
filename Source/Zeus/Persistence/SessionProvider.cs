using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using System.Reflection;
using System.Web;
using System.Collections;

namespace Zeus.Persistence
{
	public class SessionProvider : ISessionProvider
	{
		private ISessionFactory _sessionFactory;
		private IInterceptor _interceptor;

		private Dictionary<string, object> _requestItems = new Dictionary<string, object>();

		public ISession OpenSession
		{
			get
			{
				IDictionary dictionary = (HttpContext.Current != null) ? HttpContext.Current.Items : _requestItems;
				ISession session = dictionary["OpenSession"] as ISession;
				if (session == null)
					dictionary["OpenSession"] = session = _sessionFactory.OpenSession(_interceptor);
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
