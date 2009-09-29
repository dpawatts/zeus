using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[RestrictParents(typeof(Post))]
	public abstract class FeedbackItem : BaseContentItem
	{
		[ContentProperty("Approved?", 200)]
		public virtual bool Approved
		{
			get { return GetDetail("Approved", false); }
			set { SetDetail("Approved", value); }
		}

		[ContentProperty("Spam?", 210)]
		public virtual bool Spam
		{
			get { return GetDetail("Spam", false); }
			set { SetDetail("Spam", value); }
		}

		public abstract string AntiSpamAuthorName { get; }
		public abstract string AntiSpamAuthorEmail { get; }
		public abstract string AntiSpamAuthorUrl { get; }
		public abstract string AntiSpamContent { get; }
		public abstract string AntiSpamFeedbackType { get; }

		public override string IconUrl
		{
			get { return GetIconUrl(typeof(Comment), "Zeus.AddIns.Blogs.Icons.comment.png"); }
		}
	}
}