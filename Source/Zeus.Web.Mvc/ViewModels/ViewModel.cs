using Zeus.Web.UI;

namespace Zeus.Web.Mvc.ViewModels
{
	public class ViewModel<T> : IContentItemContainer<T>
		where T : ContentItem
	{
		public ViewModel(T currentItem)
		{
			CurrentItem = currentItem;
		}

		/// <summary>Gets the item associated with the item container.</summary>
		ContentItem IContentItemContainer.CurrentItem
		{
			get { return CurrentItem; }
		}

		public T CurrentItem { get; set; }
	}
}