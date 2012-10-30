namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public abstract class PostingViewModel<T> : BaseForumViewModel<T>
		where T : ContentItem
	{
		public bool CanEditSubject { get; set; }
		public string PostingFormUrl { get; set; }

		public abstract bool TopicSummaryVisible { get; }

		protected PostingViewModel(T currentItem, string postingFormUrl)
			: base(currentItem)
		{
			PostingFormUrl = postingFormUrl;
		}
	}
}