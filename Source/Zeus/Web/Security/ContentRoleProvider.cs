using System;
using System.Linq;
using System.Web.Security;
using System.Collections.Generic;

namespace Zeus.Web.Security
{
	public class ContentRoleProvider : RoleProvider
	{
		protected ItemBridge Bridge
		{
			get { return Context.Current.Resolve<ItemBridge>(); }
		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			foreach (string username in usernames)
			{
				Items.User u = Bridge.GetUser(username);
				foreach (string role in roleNames)
					if (!u.Roles.Contains(role))
					{
						u.Roles.Add(role);
						Context.Persister.Save(u);
					}
			}
		}

		private string applicationName = "Zeus.Security.Roles";

		public override string ApplicationName
		{
			get { return applicationName; }
			set { applicationName = value; }
		}

		public override void CreateRole(string roleName)
		{
			Items.UserContainer ul = Bridge.GetUserContainer(true);
			ul.AddRole(roleName);
			Context.Persister.Save(ul);
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			if (throwOnPopulatedRole && GetUsersInRole(roleName).Length > 0)
				throw new ZeusException("Role {0} cannot be deleted since it has users attached to it.", roleName);

			Items.UserContainer ul = Bridge.GetUserContainer(true);
			ul.RemoveRole(roleName);
			Context.Persister.Save(ul);
			return true;
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			IEnumerable<Items.User> users = Bridge.Finder
				.Where(ci => ci.Name == usernameToMatch)
				.Where(ci => ci["Roles"] as string == roleName)
				.OfType<Items.User>();
			return ToArray(users);
		}

		public override string[] GetAllRoles()
		{
			Items.UserContainer users = Bridge.GetUserContainer(false);
			if (users == null)
				return Bridge.DefaultRoles;
			return users.GetRoleNames();
		}

		public override string[] GetRolesForUser(string username)
		{
			Items.User u = Bridge.GetUser(username);
			if (u != null)
				return u.GetRoles();
			return new string[0];
		}

		public override string[] GetUsersInRole(string roleName)
		{
			IEnumerable<Items.User> users = Bridge.Finder
				.Where(ci => ci["Roles"] as string == roleName)
				.OfType<Items.User>();
			return ToArray(users);
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			Items.User u = Bridge.GetUser(username);
			foreach (string userRole in u.Roles)
				if (userRole.Equals(roleName))
					return true;
			return false;
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			foreach (string username in usernames)
			{
				Items.User u = Bridge.GetUser(username);
				foreach (string role in roleNames)
					if (u.Roles.Contains(role))
					{
						u.Roles.Remove(role);
						Context.Persister.Save(u);
					}
			}
		}

		public override bool RoleExists(string roleName)
		{
			return 0 < Array.IndexOf<string>(GetAllRoles(), roleName);
		}

		private static string[] ToArray(IEnumerable<Items.User> items)
		{
			return items.Select(u => u.Name).ToArray();
		}
	}
}
