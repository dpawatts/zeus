using System.Collections.Generic;
using Zeus.AddIns.Blogs.ContentTypes;

namespace Zeus.AddIns.Blogs.Services
{
	public interface ICommentService
	{
		Comment AddComment(Post post, string name, string email, string url, string text);
		void AddPingback(Post post, string sourcePageTitle, string sourceUrl);
		IEnumerable<FeedbackItem> GetDisplayedComments(Post post);
	}
}