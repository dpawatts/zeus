using Zeus.AddIns.Forums.ContentTypes;
using Zeus.BaseLibrary.Collections.Generic;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class TopicViewModel : BaseForumViewModel<Topic>
	{
		public TopicViewModel(Topic currentItem, IPagination<Post> posts, string newTopicUrl)
			: base(currentItem)
		{
			Posts = posts;
			NewTopicUrl = newTopicUrl;
		}

		public IPagination<Post> Posts { get; set; }
		public string NewTopicUrl { get; set; }
	}
}