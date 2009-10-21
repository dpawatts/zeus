using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Isis.Reflection;

namespace Zeus.Web.Security
{
	public class AuthorizationService : IAuthorizationService
	{
		private readonly AuthorizationLocation _rootLocation;

		public AuthorizationService(IAuthorizationInitializer[] authorizationInitializers)
		{
			_rootLocation = new AuthorizationLocation();
			AuthorizationRule allowRule = new AuthorizationRule { Action = AuthorizationRuleAction.Allow };
			allowRule.Users.Add("*");
			_rootLocation.Rules.Add(allowRule);

			foreach (IAuthorizationInitializer initializer in authorizationInitializers)
				initializer.Initialize(this);
		}

		public void AddRule(string locationPath, IList<string> allowUsers, IList<string> allowRoles, IList<string> denyUsers, IList<string> denyRoles)
		{
			AuthorizationLocation location = new AuthorizationLocation { Path = locationPath };

			AuthorizationRule allowRule = new AuthorizationRule { Action = AuthorizationRuleAction.Allow };
			if (allowUsers != null)
				allowRule.Users.AddRange(allowUsers);
			if (allowRoles != null)
				allowRule.Roles.AddRange(allowRoles);
			location.Rules.Add(allowRule);

			AuthorizationRule denyRule = new AuthorizationRule { Action = AuthorizationRuleAction.Deny };
			if (denyUsers != null)
				denyRule.Users.AddRange(denyUsers);
			if (denyRoles != null)
				denyRule.Roles.AddRange(denyRoles);
			location.Rules.Add(denyRule);

			_rootLocation.ChildLocations.Add(location);
		}

		public void AddRules(string locationPath, IList<AuthorizationRule> rules)
		{
			AuthorizationLocation location = new AuthorizationLocation { Path = locationPath };
			location.Rules.AddRange(rules);
			_rootLocation.ChildLocations.Add(location);
		}

		public bool CheckUrlAccessForPrincipal(string virtualPath, IPrincipal user, string verb)
		{
			if (virtualPath == null)
				throw new ArgumentNullException("virtualPath");
			if (user == null)
				throw new ArgumentNullException("user");
			if (verb == null)
				throw new ArgumentNullException("verb");

			// Check rules which pertain to this location.
			AuthorizationLocation location = (AuthorizationLocation) _rootLocation.GetChild(virtualPath);
			if (!location.EveryoneAllowed)
				return location.IsUserAllowed(user, verb);

			return true;
		}

		public bool LocationExists(string locationPath)
		{
			return _rootLocation.ChildLocations.Any(l => l.Path == locationPath);
		}
	}
}