using Coolite.Ext.Web;
using Zeus.BaseLibrary.Web;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[RestrictParents(typeof(Post))]
	public abstract class FeedbackItem : BaseContentItem
	{
		[ContentProperty("Status", 200)]
		public virtual FeedbackItemStatus Status
		{
			get { return GetDetail("Status", FeedbackItemStatus.Pending); }
			set { SetDetail("Status", value); }
		}

		[ContentProperty("Number", 220)]
		public virtual int Number
		{
			get { return GetDetail("Number", 1); }
			set { SetDetail("Number", value); }
		}

		public abstract string AntiSpamAuthorName { get; }
		public abstract string AntiSpamAuthorEmail { get; }
		public abstract string AntiSpamAuthorUrl { get; }
		public abstract string AntiSpamContent { get; }
		public abstract string AntiSpamFeedbackType { get; }

		public virtual string PostTitle
		{
			get { return Parent.Title; }
		}

		public virtual string PostUrl
		{
			get { return Parent.Url; }
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Comment); }
		}

		public override string Url
		{
			get { return new Url(Parent.Url).SetFragment("comment" + Number); }
		}
	}
}