using System.Web.Mvc;
using Zeus.Templates.ContentTypes.Widgets;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;
using Zeus.Web.Mvc;
using Zeus.Web.Security;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(LoginWidget), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class LoginWidgetController : ZeusController<LoginWidget>
	{
		[ModelStateToTempData]
		public override ActionResult Index()
		{
			return PartialView("Index", new LoginWidgetViewModel(CurrentItem, User.Identity.IsAuthenticated));
		}

		[HttpPost]
		[ModelStateToTempData]
		public ActionResult Login(LoginWidgetFormViewModel loginForm)
		{
			if (!ModelState.IsValid || !Engine.Resolve<ICredentialService>().ValidateUser(loginForm.Username, loginForm.Password))
			{
				TempData["Login.Failed"] = "Invalid username or password";
				return Redirect(CurrentItem.Parent.Url + "#loginBox");
			}

			Engine.Resolve<IAuthenticationContextService>().GetCurrentService().SetAuthCookie(loginForm.Username, false);
			return Redirect(CurrentItem.Parent.Url);
		}

		[HttpGet]
		public ActionResult Logout()
		{
			Engine.Resolve<IAuthenticationContextService>().GetCurrentService().SignOut();
			return Redirect(CurrentItem.Parent.Url);
		}
	}
}