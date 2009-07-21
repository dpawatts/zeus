using MvcContrib.Pagination;
using Zeus.AddIns.Forums.ContentTypes;

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