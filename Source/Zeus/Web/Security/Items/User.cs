using System;
using System.Linq;
using Isis.Web.UI;
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

		[ContentProperty("PasswordQuestion", 120)]
		public virtual string PasswordQuestion
		{
			get { return (string) (GetDetail("PasswordQuestion") ?? string.Empty); }
			set { SetDetail("PasswordQuestion", value, string.Empty); }
		}

		[ContentProperty("PasswordAnswer", 130)]
		public virtual string PasswordAnswer
		{
			get { return (string) (GetDetail("PasswordAnswer") ?? string.Empty); }
			set { SetDetail("PasswordAnswer", value, string.Empty); }
		}

		[ContentProperty("Is Online", 140)]
		public virtual bool IsOnline
		{
			get { return (bool) (GetDetail("IsOnline") ?? false); }
			set { SetDetail("IsOnline", value, false); }
		}

		[ContentProperty("Is Approved", 142)]
		public virtual bool IsApproved
		{
			get { return (bool) (GetDetail("IsApproved") ?? false); }
			set { SetDetail("IsApproved", value, false); }
		}

		[ContentProperty("Is Locked Out", 144)]
		public virtual bool IsLockedOut
		{
			get { return (bool) (GetDetail("IsLockedOut") ?? false); }
			set { SetDetail("IsLockedOut", value, false); }
		}

		[ContentProperty("Comment", 150)]
		public virtual string Comment
		{
			get { return (string) (GetDetail("Comment") ?? string.Empty); }
			set { SetDetail("Comment", value, string.Empty); }
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

		[ContentProperty("Last Lockout Date", 166)]
		public virtual DateTime? LastLockoutDate
		{
			get { return (DateTime?) GetDetail("LastLockoutDate"); }
			set { SetDetail("LastLockoutDate", value < new DateTime(2000, 1, 1) ? null : value); }
		}

		public override string IconUrl
		{
			get { return WebResourceUtility.GetUrl(typeof(User), "Zeus.Web.Resources.Icons.user.png"); }
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
