using Zeus.AddIns.Blogs.ContentTypes;

namespace Zeus.AddIns.Blogs.Services
{
	public interface ITagService
	{
		Tag EnsureTag(Blog blog, string tagName);
	}
}