using System;
using System.Linq;
using System.Reflection;
using NHibernate;
using Zeus.Persistence.NH.Linq;

namespace Zeus.Persistence.NH
{
	public class Finder<T> : IFinder<T>
	{
		#region Fields

		private readonly ISessionProvider _sessionProvider;

		#endregion

		#region Constructor

		public Finder(ISessionProvider sessionProvider)
		{
			_sessionProvider = sessionProvider;
		}

		#endregion

		#region IFinder<T> Members

		public IQueryable<T> Items()
		{
			return Items<T>();
		}

		public IQueryable<TResult> Items<TResult>()
			where TResult : T
		{
			return _sessionProvider.OpenSession.Session.Linq<TResult>();
		}

		public IQueryable Items(Type resultType)
		{
			ISession session = _sessionProvider.OpenSession.Session;
			MethodInfo linqMethod = typeof(ZeusExtensions).GetMethod("Linq").MakeGenericMethod(resultType);
			return (IQueryable) linqMethod.Invoke(null, new [] { session });
		}

		#endregion
	}
}
