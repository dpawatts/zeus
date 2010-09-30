using System.Collections.Generic;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class PostViewModel : ViewModel<Post>
	{
        public PostViewModel(Post currentItem, IEnumerable<FeedbackItem> comments, IEnumerable<FeedbackItem> allComments)
			: base(currentItem)
		{
			Comments = comments;
            AllComments = allComments;
		}

        public IEnumerable<FeedbackItem> Comments { get; set; }
        public IEnumerable<FeedbackItem> AllComments { get; set; }
		public string CaptchaError { get; set; }
	}
}