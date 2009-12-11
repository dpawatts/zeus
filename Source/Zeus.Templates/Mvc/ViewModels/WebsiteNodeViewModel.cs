using Zeus.Web;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class WebsiteNodeViewModel : ViewModel<WebsiteNode>
	{
		public WebsiteNodeViewModel(WebsiteNode currentItem)
			: base(currentItem)
		{
			
		}
	}
}