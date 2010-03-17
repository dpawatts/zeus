
using Zeus.Web.Mvc.ViewModels;
using Zeus.Templates.ContentTypes;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class LoginPageViewModel : ViewModel<LoginPage>
	{
		public bool LoggedIn { get; set; }

		public LoginPageViewModel(LoginPage currentItem, bool loggedIn)
			: base(currentItem)
		{
			LoggedIn = loggedIn;
		}
	}
}


