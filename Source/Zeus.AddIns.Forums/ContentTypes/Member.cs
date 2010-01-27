using System;
using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Security;
using Zeus.Templates.ContentTypes;
using Zeus.Web;
using Image = Zeus.FileSystem.Images.Image;

namespace Zeus.AddIns.Forums.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(MemberContainer))]
	[AllowedChildren(typeof(Image))]
	[Template("~/UI/Views/Forums/Member.aspx")]
	[Template("edit-profile", "~/UI/Views/Forums/EditProfile.aspx")]
	public class Member : BasePage
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.UserGreen); }
		}

		[LinkedItemDropDownListEditor("User", 10, TypeFilter = typeof(User))]
		public virtual User User
		{
			get { return GetDetail<User>("User", null); }
			set { SetDetail("User", value); }
		}

		public override string Name
		{
			get { return (User != null) ? Utility.GetSafeName(User.Username) : "[Deleted]"; }
			set { base.Name = value; }
		}

		public override string Title
		{
			get { return !string.IsNullOrEmpty(base.Title) ? base.Title : (User != null) ? User.Username : string.Empty; }
			set { base.Title = value; }
		}

		[DateEditor("Joined", 20)]
		public override DateTime Created
		{
			get { return base.Created; }
			set { base.Created = value; }
		}

		[TextBoxEditor("Full Name", 25)]
		public virtual string FullName
		{
			get { return GetDetail("FullName", string.Empty); }
			set { SetDetail("FullName", value); }
		}

		[TextBoxEditor("Signature", 30)]
		public virtual string Signature
		{
			get { return GetDetail("Signature", string.Empty); }
			set { SetDetail("Signature", value); }
		}

		[ChildEditor("Avatar", 40)]
		public virtual Image Avatar
		{
			get { return GetChild("Avatar") as Image; }
			set
			{
				if (value != null)
				{
					value.Name = "Avatar";
					value.AddTo(this);
				}
			}
		}

		public virtual int PostCount
		{
			//get { return Context.Finder.Items<Post>().Count(p => p.Author != null && p.Author == this); }
			get { return Context.Finder.QueryItems<Post>().ToList().Count(p => p.Author == this); }
		}

		public virtual IEnumerable<Forum> ModeratedForums
		{
			//get { return Context.Finder.Items<Forum>().Where(f => f.Moderator == this); }
			get { return Context.Finder.QueryItems<Forum>().ToList().Where(f => f.Moderator == this); }
		}

		public virtual IEnumerable<Topic> RecentTopics
		{
			//get { return Context.Finder.Items<Topic>().Where(t => t.Author == this).ToList().OrderByDescending(t => t.Created).Take(5); }
			get { return Context.Finder.QueryItems<Topic>().ToList().Where(t => t.Author == this).OrderByDescending(t => t.Created).Take(5); }
		}

		public virtual IEnumerable<Post> RecentReplies
		{
			//get { return Context.Finder.Items<Post>().Where(p => p.Author == this).ToList().OrderByDescending(t => t.Created).Take(5); }
			get { return Context.Finder.QueryItems<Post>().ToList().Where(p => p.Author == this).OrderByDescending(t => t.Created).Take(5); }
		}
	}
}
