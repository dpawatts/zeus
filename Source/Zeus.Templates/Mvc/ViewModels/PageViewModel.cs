using Zeus.Templates.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class PageViewModel : ViewModel<Page>
	{
		public PageViewModel(Page currentItem)
			: base(currentItem)
		{
			
		}
	}
}