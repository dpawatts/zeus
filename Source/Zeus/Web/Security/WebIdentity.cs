using System;
using System.Security.Principal;
using System.Web.Security;

namespace Zeus.Web.Security
{
	[Serializable]
	public class WebIdentity : IIdentity
	{
		public WebIdentity(FormsAuthenticationTicket ticket)
		{
			this.Ticket = ticket;
		}

		public string AuthenticationType
		{
			get { return "Web"; }
		}

		public bool IsAuthenticated
		{
			get { return true; }
		}

		public string Name
		{
			get { return this.Ticket.Name; }
		}

		public FormsAuthenticationTicket Ticket
		{
			get;
			private set;
		}
	}
}