using Zeus.Templates.ContentTypes;
using Zeus.Web;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class StartPageViewModel : ViewModel<WebsiteNode>
	{
		public StartPageViewModel(WebsiteNode currentItem)
			: base(currentItem)
		{
			
		}
	}
}