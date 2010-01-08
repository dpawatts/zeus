using System.Collections.Generic;
using Zeus.AddIns.Forums.ContentTypes;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class PostingViewModel : BaseForumViewModel<MessageBoard>
	{
		public string PostingFormUrl { get; set; }

		public IEnumerable<Post> CurrentTopicPosts { get; set; }
		public bool TopicSummaryVisible { get; set; }

		public bool CanEditSubject { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }

		public PostingViewModel(MessageBoard currentItem, string postingFormUrl)
			: base(currentItem)
		{
			PostingFormUrl = postingFormUrl;
		}
	}
}