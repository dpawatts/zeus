using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Isis.Web.Security
{
	public class AuthorizationRule
	{
		public AuthorizationRule()
		{
			Users = new List<string>();
			Roles = new List<string>();
			Verbs = new List<string>();
		}

		public AuthorizationRuleAction Action { get; set; }
		public List<string> Users { get; private set; }
		public List<string> Roles { get; private set; }
		public List<string> Verbs { get; private set; }

		public bool AllUsersSpecified
		{
			get { return Users.Any(u => u == "*"); }
		}

		public bool AnonUserSpecified
		{
			get { return Users.Any(u => u == "?"); }
		}

		public bool Everyone
		{
			get { return AllUsersSpecified && (Verbs.Count == 0); }
		}

		public int IsUserAllowed(IPrincipal user, string verb)
		{
			int num = (Action == AuthorizationRuleAction.Allow) ? 1 : -1;
			if (Everyone)
				return num;

			if (FindVerb(verb))
			{
				if (AllUsersSpecified)
					return num;

				if (AnonUserSpecified && !user.Identity.IsAuthenticated)
					return num;

				if (Users.Any(u => u == user.Identity.Name))
					return num;

				if (Roles.Any(r => user.IsInRole(r)))
					return num;
			}

			return 0;
		}

		private bool FindVerb(string verb)
		{
			if (Verbs.Count < 1)
				return true;

			return Verbs.Any(v => string.Equals(v, verb, StringComparison.OrdinalIgnoreCase));
		}
	}
}