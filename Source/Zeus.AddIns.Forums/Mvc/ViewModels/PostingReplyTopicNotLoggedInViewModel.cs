using Zeus.AddIns.Forums.ContentTypes;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class PostingReplyTopicNotLoggedInViewModel : PostingNotLoggedInViewModel<Topic>
	{
		public PostingReplyTopicNotLoggedInViewModel(Topic currentItem)
			: base(currentItem)
		{

		}
	}
}