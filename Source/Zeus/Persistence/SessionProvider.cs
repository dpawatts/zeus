using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using System.Reflection;

namespace Zeus.Persistence
{
	public class SessionProvider : ISessionProvider
	{
		private ISessionFactory _sessionFactory;

		public ISession OpenSession
		{
			get
			{
				return _sessionFactory.OpenSession();
			}
		}

		public SessionProvider(IConfigurationBuilder configurationBuilder)
		{
			_sessionFactory = configurationBuilder.Configuration.BuildSessionFactory();
		}
	}
}
