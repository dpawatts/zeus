using Zeus.Templates.Configuration;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Templates.Services;
using Zeus.Templates.Services.AntiSpam;
using Zeus.Web;
using Zeus.Web.Security;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(RegistrationPage), AreaName = TemplatesAreaRegistration.AREA_NAME)]
	public class RegistrationPageController : RegistrationPageControllerBase<RegistrationPage, RegistrationPageFormViewModel>
	{
		public RegistrationPageController(TemplatesSection templatesConfig, IUserRegistrationService userRegistrationService,
			IWebContext webContext, ICredentialService credentialService, ICaptchaService captchaService)
			: base(templatesConfig, userRegistrationService, captchaService, webContext, credentialService)
		{
		}

		protected override string VerificationEmailSender
		{
			get { return CurrentItem.VerificationEmailSender; }
		}

		protected override string VerificationEmailSubject
		{
			get { return CurrentItem.VerificationEmailSubject; }
		}

		protected override string VerificationEmailBody
		{
			get { return CurrentItem.VerificationEmailBody; }
		}
	}
}