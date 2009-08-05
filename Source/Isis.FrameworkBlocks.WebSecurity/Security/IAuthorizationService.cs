using System.Collections.Generic;
using System.Security.Principal;

namespace Isis.Web.Security
{
	public interface IAuthorizationService
	{
		void AddRule(string locationPath, IList<string> allowUsers, IList<string> allowRoles, IList<string> denyUsers, IList<string> denyRoles);
		void AddRules(string locationPath, IList<AuthorizationRule> rules);
		bool CheckUrlAccessForPrincipal(string virtualPath, IPrincipal user, string verb);
		bool LocationExists(string locationPath);
	}
}