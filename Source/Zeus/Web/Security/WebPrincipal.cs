using System.Security.Principal;
using System.Web.Security;
using Zeus.Security;

namespace Zeus.Web.Security
{
	public class WebPrincipal : GenericPrincipal
	{
		public WebPrincipal(User membershipUser, FormsAuthenticationTicket ticket)
			: base(new WebIdentity(ticket), membershipUser.Roles)
		{
			MembershipUser = membershipUser;
		}

		public User MembershipUser
		{
			get;
			private set;
		}
	}
}