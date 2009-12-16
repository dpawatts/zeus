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
			return PartialView("Index", new LoginWidgetViewModel(CurrentItem));
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

			Engine.Resolve<IAuthenticationContextService>().GetCurrentService().RedirectFromLoginPage(loginForm.Username, false);
			return new EmptyResult();
		}
	}
}