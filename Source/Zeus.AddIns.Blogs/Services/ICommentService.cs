using Zeus.AddIns.Blogs.ContentTypes;

namespace Zeus.AddIns.Blogs.Services
{
	public interface ICommentService
	{
		void AddComment(Post post, string name, string url, string text);
		void AddPingback(Post post, string sourcePageTitle, string sourceUrl);
	}
}