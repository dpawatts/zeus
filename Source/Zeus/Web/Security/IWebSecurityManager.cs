using System.Collections.Generic;
using System.Security.Principal;

namespace Zeus.Web.Security
{
	public interface IWebSecurityManager
	{
		Items.Role GetRole(string roleName);

		/// <summary>
		/// Returns the roles which the specified user can access.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		IEnumerable<Items.Role> GetRoles(IPrincipal user);
	}
}