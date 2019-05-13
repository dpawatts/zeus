
using Zeus.Web.Mvc.ViewModels;
using Zeus.Templates.ContentTypes;
using Zeus.Security;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class LoginPageViewModel : ViewModel<LoginPage>
	{
        public bool LoggedIn { get; set; }
        public User User { get; set; }

		public LoginPageViewModel(LoginPage currentItem, bool loggedIn)
			: base(currentItem)
		{
			LoggedIn = loggedIn;
        }

        public LoginPageViewModel(LoginPage currentItem, bool loggedIn, User user)
            : base(currentItem)
        {
            LoggedIn = loggedIn;
            User = user;
        }
	}
}


