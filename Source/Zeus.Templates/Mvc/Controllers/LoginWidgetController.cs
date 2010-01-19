using System.Web.Mvc;
using Zeus.Templates.ContentTypes.Widgets;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;
using Zeus.Web.Mvc.ActionFilters;
using Zeus.Web.Security;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(LoginWidget), AreaName = TemplatesAreaRegistration.AREA_NAME)]
	public class LoginWidgetController : WidgetController<LoginWidget>
	{
		private readonly IWebSecurityService _webSecurityService;

		public LoginWidgetController(IWebSecurityService webSecurityService)
		{
			_webSecurityService = webSecurityService;
		}

		[ImportModelStateFromTempData]
		public override ActionResult Index()
		{
			return PartialView("Index", new LoginWidgetViewModel(CurrentItem, User.Identity.IsAuthenticated));
		}

		[HttpPost]
		[ExportModelStateToTempData]
		public ActionResult Login(LoginWidgetFormViewModel loginForm)
		{
			if (!ModelState.IsValid || !_webSecurityService.ValidateUser(loginForm.Username, loginForm.Password))
			{
				ModelState.AddModelError("Login.Failed", "Invalid username or password");
				return RedirectToParentPage("#loginBox");
			}

			_webSecurityService.SetAuthCookie(loginForm.Username, false);
			return RedirectToParentPage();
		}

		[HttpGet]
		public ActionResult Logout()
		{
			_webSecurityService.SignOut();
			return RedirectToParentPage();
		}
	}
}