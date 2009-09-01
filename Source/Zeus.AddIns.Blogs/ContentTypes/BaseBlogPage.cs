using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	public abstract class BaseBlogPage : BasePage
	{
		public abstract Blog CurrentBlog { get; }
	}
}