using System;
using System.Security.Principal;

namespace Zeus.Security
{
	/// <summary>
	/// Manages security by subscribing to persister events and providing 
	/// methods to authorize request event.
	/// </summary>
	public class SecurityManager : ISecurityManager
	{
		/// <summary>Find out if a principal is allowed to access an item.</summary>
		/// <param name="item">The item to check against.</param>
		/// <param name="principal">The principal to check for allowance.</param>
		/// <returns>True if the item has public access or the principal is allowed to access it.</returns>
		public virtual bool IsAuthorized(ContentItem item, IPrincipal principal)
		{
			if (principal != null && principal.Identity.Name == "administrator")
				return true;

			if (principal != null && principal.IsInRole("Administrator"))
				return true;

			return item.IsAuthorised(principal);
		}
	}
}
