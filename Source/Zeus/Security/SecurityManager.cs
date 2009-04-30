using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Zeus.Engine;

namespace Zeus.Security
{
	/// <summary>
	/// Manages security by subscribing to persister events and providing 
	/// methods to authorize request event.
	/// </summary>
	public class SecurityManager : ISecurityManager
	{
		#region Fields

		private IPluginFinder<AvailableOperationAttribute> _operationFinder;

		#endregion

		#region Constructor

		public SecurityManager(IPluginFinder<AvailableOperationAttribute> operationFinder)
		{
			_operationFinder = operationFinder;
		}

		#endregion

		#region Methods

		public IEnumerable<string> GetAuthorizedRoles(ContentItem item)
		{
			return item.AuthorizationRules.Where(ar => ar.Role != null).Select(ar => ar.Role).Distinct();
		}

		public IEnumerable<string> GetAuthorizedUsers(ContentItem item)
		{
			return item.AuthorizationRules.Where(ar => ar.User != null).Select(ar => ar.User).Distinct();
		}

		public IEnumerable<string> GetAvailableOperations()
		{
			return _operationFinder.GetPlugins().OrderBy(ao => ao.SortOrder).Select(ao => ao.Name);
		}

		/// <summary>Find out if a principal is allowed to access an item.</summary>
		/// <param name="item">The item to check against.</param>
		/// <param name="principal">The principal to check for allowance.</param>
		/// <param name="operation"></param>
		/// <returns>True if the item has public access or the principal is allowed to access it.</returns>
		public virtual bool IsAuthorized(ContentItem item, IPrincipal principal, string operation)
		{
			if (principal != null && string.Equals(principal.Identity.Name, "administrator", StringComparison.OrdinalIgnoreCase))
				return true;

			if (principal != null && principal.IsInRole("Administrators"))
				return true;

			return item.IsAuthorized(principal, operation);
		}

		#endregion
	}
}
