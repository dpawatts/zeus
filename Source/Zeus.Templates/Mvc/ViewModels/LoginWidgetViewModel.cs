using Zeus.Templates.ContentTypes.Widgets;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class LoginWidgetViewModel : ViewModel<LoginWidget>
	{
		public LoginWidgetViewModel(LoginWidget currentItem)
			: base(currentItem)
		{
			
		}
	}
}


