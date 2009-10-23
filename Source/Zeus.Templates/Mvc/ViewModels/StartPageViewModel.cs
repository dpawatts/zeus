using Zeus.Templates.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class StartPageViewModel : ViewModel<StartPage>
	{
		public StartPageViewModel(StartPage currentItem)
			: base(currentItem)
		{
			
		}
	}
}