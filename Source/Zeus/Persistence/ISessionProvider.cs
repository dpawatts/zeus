using System;
using NHibernate;

namespace Zeus.Persistence
{
	public interface ISessionProvider
	{
		ISession OpenSession
		{
			get;
		}
	}
}
