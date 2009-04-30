using NHibernate;
using Zeus.Web;

namespace Zeus.Persistence.NH
{
	public class SessionContext : IClosable
	{
		private readonly ISessionProvider provider;

		public ISession Session
		{
			get;
			set;
		}

		/// <summary>
		/// Indicates wheter content has changed during the request.
		/// </summary>
		public bool ContentChanged
		{
			get;
			set;
		}

		public SessionContext(ISessionProvider provider, ISession session)
		{
			this.provider = provider;
			Session = session;
		}

		#region IDisposable Members

		public void Dispose()
		{
			provider.Dispose();
		}

		#endregion
	}
}
