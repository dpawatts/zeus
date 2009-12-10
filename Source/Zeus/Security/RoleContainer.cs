using System.Linq;
using Coolite.Ext.Web;
using Zeus.ContentTypes;
using Zeus.Integrity;
using Zeus.Security.ContentTypes;

namespace Zeus.Web.Security.Items
{
	[ContentType("Role Container")]
	[RestrictParents(typeof(SecurityContainer))]
	public class RoleContainer : ContentItem
	{
		public const string ContainerName = "roles";
		public const string ContainerTitle = "Roles";

		public RoleContainer()
		{
			Name = ContainerName;
			Title = ContainerTitle;
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.GroupKey); }
		}

		public virtual string[] GetRoleNames()
		{
			return GetChildren<Role>().Select(r => r.Name).ToArray();
		}

		/// <summary>Adds a role if not already present.</summary>
		/// <param name="roleName">The role to add.</param>
		public virtual void AddRole(string roleName)
		{
			Role existingRole = Children.Cast<Role>().SingleOrDefault(ci => ci.Name == roleName);
			if (existingRole != null)
				return;

			Role role = new Role { Name = roleName };
			role.AddTo(this);
		}

		/// <summary>Removes a role if existing.</summary>
		/// <param name="roleName">The role to remove.</param>
		public virtual void RemoveRole(string roleName)
		{
			Role role = Children.Cast<Role>().SingleOrDefault(ci => ci.Name == roleName);
			if (role != null)
				Children.Remove(role);
		}

		public virtual Role GetRole(string roleName)
		{
			Role role = GetChild(roleName) as Role;
			if (role == null)
				throw new ZeusException("Role '{0}' does not exist.", roleName);
			return role;
		}
	}
}
