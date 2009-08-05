using System;
using System.Security.Principal;

namespace Isis.Web.Security
{
	[Serializable]
	public class WebIdentity : IIdentity
	{
		public WebIdentity(AuthenticationTicket ticket)
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

		public AuthenticationTicket Ticket
		{
			get;
			private set;
		}
	}
}
