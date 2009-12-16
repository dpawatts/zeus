using Zeus.Templates.Configuration;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Templates.Services.AntiSpam;
using Zeus.Web;
using Zeus.Web.Security;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(RegistrationPage), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class RegistrationPageController : RegistrationPageControllerBase<RegistrationPageFormViewModel>
	{
		public RegistrationPageController(TemplatesSection templatesConfig, IWebContext webContext,
			ICredentialService credentialService, ICaptchaService captchaService)
			: base(templatesConfig, webContext, credentialService, captchaService)
		{
		}
	}
}