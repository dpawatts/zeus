using System;
using System.Web;
using System.Security.Principal;

namespace Zeus.Web.Security
{
	public class AuthenticationEventArgs : EventArgs
	{
		public AuthenticationEventArgs(HttpContextBase context)
		{
			this.Context = context;
		}

		public HttpContextBase Context
		{
			get;
			private set;
		}

		public IPrincipal User
		{
			get;
			set;
		}
	}
}