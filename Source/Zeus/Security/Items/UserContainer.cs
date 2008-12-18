using System;
using System.Linq;
using Zeus.ContentTypes.Properties;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Security;
using Zeus.Linq.Filters;

namespace Zeus.Security.Items
{
	[ContentType("User Container")]
	public class UserContainer : ContentItem
	{
		[TextBoxEditor("Title", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		public override string IconUrl
		{
			get { return "~/Admin/Assets/Images/Icons/group.png"; }
		}

		[TextBoxEditor("Roles", 100, TextMode = TextBoxMode.MultiLine)]
		public virtual string Roles
		{
			get { return (string) (GetDetail("Roles") ?? "Everyone"); }
			set { SetDetail("Roles", value, "Everyone"); }
		}

		public virtual string[] GetRoleNames()
		{
			return Roles.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		}

		/// <summary>Adds a role if not already present.</summary>
		/// <param name="roleName">The role to add.</param>
		public virtual void AddRole(string roleName)
		{
			if (Array.IndexOf<string>(GetRoleNames(), roleName) < 0)
				Roles += Environment.NewLine + roleName;
		}

		/// <summary>Removes a role if existing.</summary>
		/// <param name="roleName">The role to remove.</param>
		public virtual void RemoveRole(string roleName)
		{
			List<string> roles = new List<string>();
			foreach (string existingRole in GetRoleNames())
				if (!existingRole.Equals(roleName))
					roles.Add(existingRole);
			Roles = string.Join(Environment.NewLine, roles.ToArray());
		}


		public virtual MembershipUserCollection GetMembershipUsers(string providerName)
		{
			MembershipUserCollection muc = new MembershipUserCollection();
			foreach (User u in Children)
				muc.Add(u.GetMembershipUser(providerName));
			return muc;
		}

		public virtual MembershipUserCollection GetMembershipUsers(string providerName, int startIndex, int maxResults, out int totalRecords)
		{
			totalRecords = 0;
			CountFilter countFilter = new CountFilter(startIndex, maxResults);
			MembershipUserCollection muc = new MembershipUserCollection();
			foreach (User u in countFilter.Filter(Children.AsQueryable()))
			{
				muc.Add(u.GetMembershipUser(providerName));
				totalRecords++;
			}
			return muc;
		}
	}
}
