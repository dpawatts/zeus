using Zeus.AddIns.Forums.ContentTypes;
using Zeus.BaseLibrary.Collections.Generic;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class TopicViewModel : BaseForumViewModel<Topic>
	{
		public TopicViewModel(Topic currentItem, IPagination<Post> posts)
			: base(currentItem)
		{
			Posts = posts;
		}

		public IPagination<Post> Posts { get; set; }
	}
}