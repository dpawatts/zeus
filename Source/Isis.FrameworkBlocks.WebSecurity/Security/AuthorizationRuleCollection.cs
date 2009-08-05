using System.Collections.Generic;
using System.Security.Principal;

namespace Isis.Web.Security
{
	public class AuthorizationRuleCollection : List<AuthorizationRule>
	{
		public bool IsUserAllowed(IPrincipal user, string verb)
		{
			if (user != null)
			{
				foreach (AuthorizationRule rule in this)
				{
					int num = rule.IsUserAllowed(user, verb);
					if (num != 0)
						return (num > 0);
				}
			}
			return false;
		}
	}
}