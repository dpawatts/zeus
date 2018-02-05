using Zeus.Templates.ContentTypes.Widgets;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class LoginWidgetViewModel : ViewModel<LoginWidget>
	{
		public bool LoggedIn { get; set; }

		public LoginWidgetViewModel(LoginWidget currentItem, bool loggedIn)
			: base(currentItem)
		{
			LoggedIn = loggedIn;
		}
	}
}


