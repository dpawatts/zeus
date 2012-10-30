namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public abstract class PostingNotLoggedInViewModel<T> : BaseForumViewModel<T>
		where T : ContentItem
	{
		protected PostingNotLoggedInViewModel(T currentItem)
			: base(currentItem)
		{
			
		}
	}
}