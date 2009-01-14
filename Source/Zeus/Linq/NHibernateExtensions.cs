using System;
using NHibernate.Linq;
using NHibernate;

namespace Zeus.Linq
{
	public static class NHibernateExtensions
	{
		public static INHibernateQueryable<T> Linq2<T>(this ISession session)
		{
			QueryOptions options = new QueryOptions();
			return new Query<T>(new ZeusProvider(session, options), options);
		}
	}
}
