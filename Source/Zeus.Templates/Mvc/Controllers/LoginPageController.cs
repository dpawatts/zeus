using System.Web.Mvc;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;
using Zeus.Web.Mvc.ActionFilters;
using Zeus.Web.Security;
using Zeus.Templates.ContentTypes;
using System.Net;

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
            return View(new LoginPageViewModel(CurrentItem, User.Identity.IsAuthenticated, CurrentUser));
		}

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Login(LoginPageFormViewModel loginForm)
		{
            if (!ModelState.IsValid || !_webSecurityService.ValidateUser(loginForm.Username, loginForm.Password))
            {
                ModelState.AddModelError("Login.Failed", "Invalid username or password");
                if (!string.IsNullOrEmpty(loginForm.Target))
                    return RedirectToParentPage("?target=" + loginForm.Target + "#loginBox");
            }
            else
            {
                _webSecurityService.SetAuthCookie(loginForm.Username, false);

                //check for redirect instruction, and check that it isn't going to error - if it is then ignore the redirect
                if (!string.IsNullOrEmpty(loginForm.Target))
                {
                    if (TestFor500(loginForm.Target))
                        Response.Redirect(loginForm.Target);
                }
            }

            return RedirectToParentPage();
		}

        public static bool TestFor500(string url)
        {
            try
            {
                HttpWebRequest webRequest = HttpWebRequest.Create("http://" + System.Web.HttpContext.Current.Request.Url.Host + url) as HttpWebRequest;
                webRequest.Method = WebRequestMethods.Http.Get;
                webRequest.ContentType = "application/x-www-form-urlencoded";
                using (HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }
	}
}