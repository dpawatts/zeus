using System.Security.Principal;

namespace Zeus.Web.Security
{
	public class WebPrincipal : GenericPrincipal
	{
		public WebPrincipal(IUser membershipUser, AuthenticationTicket ticket)
			: base(new WebIdentity(ticket), membershipUser.Roles)
		{
			MembershipUser = membershipUser;
		}

		public IUser MembershipUser
		{
			get;
			private set;
		}
	}
}