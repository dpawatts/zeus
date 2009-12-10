using System;
using System.Linq;
using Coolite.Ext.Web;
using Zeus.BaseLibrary.Web.UI;
using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Web.Security.Details;

namespace Zeus.Web.Security.Items
{
	[ContentType]
	[RestrictParents(typeof(UserContainer))]
	public class User : ContentItem, IUser
	{
		public override string Title
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		public string Identifier
		{
			get { return Name; }
		}

		[TextBoxEditor("UserName", 20, Required = true)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		[ContentProperty("Password", 30)]
		public virtual string Password
		{
			get { return (string) (GetDetail("Password") ?? string.Empty); }
			set { SetDetail("Password", value, string.Empty); }
		}

		[ContentProperty("Email", 40)]
		public virtual string Email
		{
			get { return (string) (GetDetail("Email") ?? string.Empty); }
			set { SetDetail("Email", value, string.Empty); }
		}

		[RolesEditor(Title = "Roles", SortOrder = 50)]
		public virtual PropertyCollection Roles
		{
			get { return GetDetailCollection("Roles", true); }
		}

		string[] IUser.Roles
		{
			get { return Roles.Cast<Role>().Select(r => r.Name).ToArray(); }
		}

		[ContentProperty("Nonce", 141)]
		public virtual string Nonce
		{
			get { return GetDetail("Nonce", string.Empty); }
			set { SetDetail("Nonce", value); }
		}

		[ContentProperty("Verified", 142)]
		public virtual bool Verified
		{
			get { return GetDetail("Verified", false); }
			set { SetDetail("Verified", value); }
		}

		[ContentProperty("Last Login Date", 160)]
		public virtual DateTime LastLoginDate
		{
			get { return (DateTime) (GetDetail("LastLoginDate") ?? Published.Value); }
			set { SetDetail("LastLoginDate", value, Published.Value); }
		}

		[ContentProperty("Last Activity Date", 162)]
		public virtual DateTime LastActivityDate
		{
			get { return (DateTime) (GetDetail("LastActivityDate") ?? Published.Value); }
			set { SetDetail("LastActivityDate", value, Published.Value); }
		}

		[ContentProperty("Last Password Changed Date", 164)]
		public virtual DateTime LastPasswordChangedDate
		{
			get { return (DateTime) (GetDetail("LastPasswordChangedDate") ?? Published.Value); }
			set { SetDetail("LastPasswordChangedDate", value, Published.Value); }
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.User); }
		}

		string IUser.Username
		{
			get { return Name; }
			set { Name = value; }
		}

		string IUser.Password
		{
			get { return Password; }
			set { Password = value; }
		}

		public virtual string[] GetRoles()
		{
			string[] roles = new string[Roles.Count];
			for (int i = 0; i < roles.Length; i++)
				roles[i] = Roles[i] as string;
			return roles;
		}
	}
}
