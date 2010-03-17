using System.Web.Mvc;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;
using Zeus.Web.Mvc.ActionFilters;
using Zeus.Web.Security;
using Zeus.Templates.ContentTypes;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(LoginPage), AreaName = TemplatesAreaRegistration.AREA_NAME)]
    public class LoginPageController : ZeusController<LoginPage>
	{
		private readonly IWebSecurityService _webSecurityService;

        public LoginPageController(IWebSecurityService webSecurityService)
		{
			_webSecurityService = webSecurityService;
		}

        [ImportModelStateFromTempData]
        public override ActionResult Index()
		{
			return View(new LoginPageViewModel(CurrentItem, User.Identity.IsAuthenticated));
		}

        
		[HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Login(LoginPageFormViewModel loginForm)
		{
			if (!ModelState.IsValid || !_webSecurityService.ValidateUser(loginForm.Username, loginForm.Password))
			{
				ModelState.AddModelError("Login.Failed", "Invalid username or password");
				return RedirectToParentPage("#loginBox");
			}

			_webSecurityService.SetAuthCookie(loginForm.Username, false);
			return RedirectToParentPage();
		}
        
	}
}