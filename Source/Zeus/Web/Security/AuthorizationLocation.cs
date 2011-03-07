using System.Security.Principal;

namespace Zeus.Web.Security
{
	public class AuthorizationLocation : SecurityLocation
	{
		public AuthorizationLocation()
		{
			Rules = new AuthorizationRuleCollection();
		}

		public AuthorizationRuleCollection Rules { get; private set; }

		public bool EveryoneAllowed
		{
			get { return Rules.Count > 0 && Rules[0].Action == AuthorizationRuleAction.Allow && Rules[0].Everyone; }
		}

		public bool IsUserAllowed(IPrincipal user, string verb)
		{
			return Rules.IsUserAllowed(user, verb);
		}
	}
}