using System.Collections.Generic;
using Zeus.AddIns.Forums.ContentTypes;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class ReplyTopicPostingViewModel : PostingViewModel<Topic>
	{
		public ReplyTopicPostingViewModel(Topic currentItem, string postingFormUrl, string newTopicUrl, IEnumerable<Post> currentTopicPosts)
			: base(currentItem, postingFormUrl)
		{
			NewTopicUrl = newTopicUrl;
			CurrentTopicPosts = currentTopicPosts;
		}

		public IEnumerable<Post> CurrentTopicPosts { get; set; }

		public override bool TopicSummaryVisible
		{
			get { return true; }
		}

		public string NewTopicUrl { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }		
	}
}