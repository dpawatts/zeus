using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Zeus.Configuration;
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

		private readonly IPluginFinder<AvailableOperationAttribute> _operationFinder;
		private readonly AdminSection _adminConfig;

		#endregion

		#region Constructor

		public SecurityManager(IPluginFinder<AvailableOperationAttribute> operationFinder, AdminSection adminConfig)
		{
			_operationFinder = operationFinder;
			_adminConfig = adminConfig;
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
		/// <param name="user">The principal to check for authorization.</param>
		/// <param name="operation"></param>
		/// <returns>True if the item has public access or the principal is allowed to access it.</returns>
		public virtual bool IsAuthorized(ContentItem item, IPrincipal user, string operation)
		{
			if (user != null && IsAdmin(user))
				return true;

			if (!IsPublished(item) && operation == Operations.Read)
				operation = Operations.ReadUnpublished;

			return item.IsAuthorized(user, operation);
		}

		/// <summary>Check whether an item is published, i.e. it's published and isn't expired.</summary>
		/// <param name="item">The item to check.</param>
		/// <returns>A boolean indicating whether the item is published.</returns>
		public virtual bool IsPublished(ContentItem item)
		{
			return (item.Published.HasValue && DateTime.Now >= item.Published)
				&& (!item.Expires.HasValue || DateTime.Now < item.Expires.Value);
		}

		/// <summary>Find out if a princpial has admin access.</summary>
		/// <param name="user">The princpial to check.</param>
		/// <returns>A boolean indicating whether the principal has admin access.</returns>
		public bool IsAdmin(IPrincipal user)
		{
			return user.IsInRole(_adminConfig.AdministratorRole);
		}

		#endregion
	}
}
