using Zeus.AddIns.Blogs.ContentTypes;

namespace Zeus.AddIns.Blogs.Services
{
	public interface ICommentService
	{
		void AddComment(Post post, string name, string url, string text);
	}
}