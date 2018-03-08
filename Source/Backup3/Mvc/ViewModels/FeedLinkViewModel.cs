using Zeus.Templates.ContentTypes.Widgets;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class FeedLinkViewModel : ViewModel<FeedLink>
	{
		public FeedLinkViewModel(FeedLink currentItem)
			: base(currentItem)
		{
			
		}
	}
}