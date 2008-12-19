using System;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using System.Web.Security;
using Zeus.Web.Security.Details;

namespace Zeus.Web.Security.Items
{
	[ContentType]
	[RestrictParents(typeof(UserContainer))]
	public class User : ContentItem
	{
		[TextBoxEditor("Title", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[TextBoxEditor("Title", 20, Required = true)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		[TextBoxEditor("Password", 30)]
		public virtual string Password
		{
			get { return (string) (GetDetail("Password") ?? string.Empty); }
			set { SetDetail("Password", value, string.Empty); }
		}

		[TextBoxEditor("Email", 40)]
		public virtual string Email
		{
			get { return (string) (GetDetail("Email") ?? string.Empty); }
			set { SetDetail("Email", value, string.Empty); }
		}

		[RolesEditor(Title = "Roles", SortOrder = 50)]
		public virtual DetailCollection Roles
		{
			get { return GetDetailCollection("Roles", true); }
		}

		[TextBoxEditor("PasswordQuestion", 120)]
		public virtual string PasswordQuestion
		{
			get { return (string) (GetDetail("PasswordQuestion") ?? string.Empty); }
			set { SetDetail("PasswordQuestion", value, string.Empty); }
		}

		[TextBoxEditor("PasswordAnswer", 130)]
		public virtual string PasswordAnswer
		{
			get { return (string) (GetDetail("PasswordAnswer") ?? string.Empty); }
			set { SetDetail("PasswordAnswer", value, string.Empty); }
		}

		[CheckBoxEditor("Is Online", "", 140)]
		public virtual bool IsOnline
		{
			get { return (bool) (GetDetail("IsOnline") ?? false); }
			set { SetDetail("IsOnline", value, false); }
		}

		[CheckBoxEditor("Is Approved", "", 142)]
		public virtual bool IsApproved
		{
			get { return (bool) (GetDetail("IsApproved") ?? false); }
			set { SetDetail("IsApproved", value, false); }
		}

		[CheckBoxEditor("Is Locked Out", "", 144)]
		public virtual bool IsLockedOut
		{
			get { return (bool) (GetDetail("IsLockedOut") ?? false); }
			set { SetDetail("IsLockedOut", value, false); }
		}

		public virtual object ProviderUserKey
		{
			get { return GetDetail("ProviderUserKey"); }
			set { SetDetail("ProviderUserKey", value); }
		}

		[TextBoxEditor("Comment", 150)]
		public virtual string Comment
		{
			get { return (string) (GetDetail("Comment") ?? string.Empty); }
			set { SetDetail("Comment", value, string.Empty); }
		}

		[TextBoxEditor("Last Login Date", 160)]
		public virtual DateTime LastLoginDate
		{
			get { return (DateTime) (GetDetail("LastLoginDate") ?? Published.Value); }
			set { SetDetail("LastLoginDate", value, Published.Value); }
		}

		[TextBoxEditor("Last Activity Date", 162)]
		public virtual DateTime LastActivityDate
		{
			get { return (DateTime) (GetDetail("LastActivityDate") ?? Published.Value); }
			set { SetDetail("LastActivityDate", value, Published.Value); }
		}

		[TextBoxEditor("Last Password Changed Date", 164)]
		public virtual DateTime LastPasswordChangedDate
		{
			get { return (DateTime) (GetDetail("LastPasswordChangedDate") ?? Published.Value); }
			set { SetDetail("LastPasswordChangedDate", value, Published.Value); }
		}

		[TextBoxEditor("Last Lockout Date", 166)]
		public virtual DateTime? LastLockoutDate
		{
			get { return (DateTime?) GetDetail("LastLockoutDate"); }
			set { SetDetail("LastLockoutDate", value < new DateTime(2000, 1, 1) ? null : value); }
		}

		public override bool IsPage
		{
			get { return false; }
		}

		public override string IconUrl
		{
			get { return "~/Admin/Assets/Images/Icons/user.png"; }
		}

		public virtual MembershipUser GetMembershipUser(string providerName)
		{
			return
				new MembershipUser(providerName, Name, ProviderUserKey, Email, PasswordQuestion, Comment, IsApproved, IsLockedOut,
					Created, LastLoginDate, LastActivityDate, LastPasswordChangedDate,
					(LastLockoutDate ?? DateTime.MinValue));
		}

		public virtual void UpdateFromMembershipUser(MembershipUser mu)
		{
			Comment = mu.Comment;
			Created = mu.CreationDate;
			Email = mu.Email;
			IsApproved = mu.IsApproved;
			IsLockedOut = mu.IsLockedOut;
			IsOnline = mu.IsOnline;
			LastActivityDate = mu.LastActivityDate;
			LastLockoutDate = mu.LastLockoutDate;
			LastLoginDate = mu.LastLoginDate;
			LastPasswordChangedDate = mu.LastPasswordChangedDate;
			PasswordQuestion = mu.PasswordQuestion;
			ProviderUserKey = mu.ProviderUserKey;
			Name = mu.UserName;
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
