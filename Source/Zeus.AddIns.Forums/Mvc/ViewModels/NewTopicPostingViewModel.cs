using Zeus.AddIns.Forums.ContentTypes;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class NewTopicPostingViewModel : PostingViewModel<Forum>
	{
		public NewTopicPostingViewModel(Forum currentItem, string postingFormUrl)
			: base(currentItem, postingFormUrl)
		{
			
		}

		public override bool TopicSummaryVisible
		{
			get { return false; }
		}
	}
}