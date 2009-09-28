using System.Collections.Generic;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class PostViewModel : ViewModel<Post>
	{
		public PostViewModel(Post currentItem, IEnumerable<FeedbackItem> comments)
			: base(currentItem)
		{
			Comments = comments;
		}

		public IEnumerable<FeedbackItem> Comments { get; set; }
		public string CaptchaError { get; set; }
	}
}