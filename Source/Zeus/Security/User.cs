using System;
using System.Linq;
using Ext.Net;
using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Web.Security.Details;
using Zeus.Web.Security.Items;

namespace Zeus.Security
{
	[ContentType]
	[RestrictParents(typeof(UserContainer))]
	public class User : DataContentItem
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

		[TextBoxEditor("Username", 20, Required = true)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		public string Username
		{
			get { return Name; }
			set { Name = value; }
		}

		[ContentProperty("Change Password", 30, Description = "Passwords are encrypted and cannot be viewed, but they can be changed.")]
		[PasswordEditor]
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

		[RolesEditor(Title="Roles", SortOrder = 50)]
		public virtual PropertyCollection RolesInternal
		{
			get { return GetDetailCollection("RolesInternal", true); }
		}

		public string[] Roles
		{
			get { return RolesInternal.Cast<Role>().Select(r => r.Name).ToArray(); }
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
			get { return GetDetail("Verified", true); }
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

		[ContentProperty("Last Password Changed Date", 166)]
		public virtual int LoginFailureCounter
		{
			get
			{
				return (int)GetDetail("LoginFailureCounter", 0);
			}
			set
			{
				SetDetail("LoginFailureCounter", value, 0);
			}
		}

		[ContentProperty("Is Account Locked", 168)]
		public virtual bool AccountLocked
		{
			get
			{
				return (bool)GetDetail("AccountLocked", false);
			}
			set
			{
				SetDetail("AccountLocked", value, false);
			}
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.User); }
		}

        public override string FolderPlacementGroup
        {
            get
            {
                if (Roles.Count() > 1)
                    return "Multiple Roles";
                else if (Roles.Count() ==1)
                    return Roles.First();
                else
                    return "No Roles Defined";

            }
        }
	}
}
