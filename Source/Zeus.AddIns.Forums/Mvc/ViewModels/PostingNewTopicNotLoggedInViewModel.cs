using Zeus.AddIns.Forums.ContentTypes;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class PostingNewTopicNotLoggedInViewModel : PostingNotLoggedInViewModel<Forum>
	{
		public PostingNewTopicNotLoggedInViewModel(Forum currentItem)
			: base(currentItem)
		{

		}
	}
}