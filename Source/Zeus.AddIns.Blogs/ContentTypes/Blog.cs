using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType]
	public class Blog : BasePage
	{
		public override string IconUrl
		{
			get { return GetIconUrl(typeof(Blog), "Zeus.AddIns.Blogs.Icons.user_comment.png"); }
		}
	}
}