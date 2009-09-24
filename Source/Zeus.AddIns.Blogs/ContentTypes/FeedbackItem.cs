using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[RestrictParents(typeof(Post))]
	public abstract class FeedbackItem : BaseContentItem
	{
		public override string IconUrl
		{
			get { return GetIconUrl(typeof(Comment), "Zeus.AddIns.Blogs.Icons.comment.png"); }
		}
	}
}