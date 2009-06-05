using System;
using System.Collections.Generic;
using System.Linq;
using Isis.Web.Security;
using Isis.Web.UI;
using Zeus.Design.Editors;
using Zeus.FileSystem.Images;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.AddIns.Forums.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(MemberContainer))]
	[Template("~/UI/Views/Forums/Member.aspx")]
	[Template("edit-profile", "~/UI/Views/Forums/EditProfile.aspx")]
	public class Member : BasePage
	{
		public override string IconUrl
		{
			get { return WebResourceUtility.GetUrl(typeof(Member), "Zeus.AddIns.Forums.Web.Resources.user_green.png"); }
		}

		[LinkedItemDropDownListEditor("User", 10, TypeFilter = typeof(IUser))]
		public virtual IUser User
		{
			get { return GetDetail<IUser>("User", null); }
			set { SetDetail("User", value); }
		}

		public override string Name
		{
			get { return (User != null) ? Utility.GetSafeName(User.Username) : "[Deleted]"; }
			set { base.Name = value; }
		}

		public override string Title
		{
			get { return !string.IsNullOrEmpty(base.Title) ? base.Title : User.Username; }
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

		[ImageDataUploadEditor("Avatar", 40)]
		public virtual ImageData Avatar
		{
			get { return GetDetail<ImageData>("Image", null); }
			set { SetDetail("Image", value); }
		}

		public virtual int PostCount
		{
			get { return Context.Finder.Items<Post>().Count(p => p.Author == this); }
		}

		public virtual IEnumerable<Forum> ModeratedForums
		{
			get { return Context.Finder.Items<Forum>().Where(f => f.Moderator == this); }
		}

		public virtual IEnumerable<Topic> RecentTopics
		{
			get { return Context.Finder.Items<Topic>().Where(t => t.Author == this).ToList().OrderByDescending(t => t.Created).Take(5); }
		}

		public virtual IEnumerable<Post> RecentReplies
		{
			get { return Context.Finder.Items<Post>().Where(p => p.Author == this).ToList().OrderByDescending(t => t.Created).Take(5); }
		}
	}
}
