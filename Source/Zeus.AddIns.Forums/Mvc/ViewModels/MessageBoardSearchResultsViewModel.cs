using Zeus.AddIns.Forums.ContentTypes;
using Zeus.BaseLibrary.Collections.Generic;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class MessageBoardSearchResultsViewModel : BaseForumViewModel<MessageBoard>
	{
		public MessageBoardSearchResultsViewModel(MessageBoard currentItem)
			: base(currentItem)
		{

		}

		public IPagination<Post> SearchResults { get; set; }
		public int SearchResultCount { get; set; }
		public string SearchText { get; set; }
	}
}