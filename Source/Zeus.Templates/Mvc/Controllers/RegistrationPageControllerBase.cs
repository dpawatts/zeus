using System;
using System.Web.Mvc;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.BaseLibrary.Web;
using Zeus.Security;
using Zeus.Templates.Configuration;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Templates.Services.AntiSpam;
using Zeus.Web.Security;
using IWebContext = Zeus.Web.IWebContext;

namespace Zeus.Templates.Mvc.Controllers
{
	public abstract class RegistrationPageControllerBase<TFormViewModel> : ZeusController<RegistrationPage>
		where TFormViewModel : RegistrationPageFormViewModel
	{
		private readonly TemplatesSection _templatesConfig;
		private readonly IWebContext _webContext;
		private readonly ICredentialService _credentialService;
		private readonly ICaptchaService _captchaService;

		protected RegistrationPageControllerBase(TemplatesSection templatesConfig, IWebContext webContext,
			ICredentialService credentialService, ICaptchaService captchaService)
		{
			_templatesConfig = templatesConfig;
			_webContext = webContext;
			_credentialService = credentialService;
			_captchaService = captchaService;
		}

		public override ActionResult Index()
		{
			return View("Index", new RegistrationPageViewModel(CurrentItem));
		}

		[HttpPost]
		public ActionResult Register(TFormViewModel registrationForm)
		{
			if (!ModelState.IsValid)
				return Index();

			string captchaError;
			if (!_captchaService.Check(HttpContext, out captchaError))
				return View("Index", new RegistrationPageViewModel(CurrentItem) { CaptchaError = captchaError });

			// Create user.
			UserCreateStatus status;
			User user = _credentialService.CreateUser(registrationForm.Username,
				registrationForm.Password, registrationForm.Email,
				new[] { _templatesConfig.UserRegistration.DefaultRole },
				!_templatesConfig.UserRegistration.EmailVerificationRequired,
				out status);
			if (status != UserCreateStatus.Success)
			{
				ModelState.AddModelError("RegistrationError", "Could not create user: " + status.GetDescription());
				return Index();
			}

			// Allow inherited classes to create profile.
			CreateProfile(user);

			// Send verification email.
			if (_templatesConfig.UserRegistration.EmailVerificationRequired)
				_credentialService.SendVerificationEmail(user,
					_webContext.GetFullyQualifiedUrl(new Url(CurrentItem.Url).AppendSegment("verify").AppendQuery("n=")),
					registrationForm.Email, CurrentItem.VerificationEmailSender, CurrentItem.VerificationEmailSubject,
					CurrentItem.VerificationEmailBody);

			return View("RegisterConfirmation", new RegistrationPageConfirmationViewModel(
				CurrentItem, _templatesConfig.UserRegistration.EmailVerificationRequired,
				registrationForm.Email));
		}

		protected virtual void CreateProfile(User user)
		{

		}

		[HttpGet]
		public ActionResult Verify(string n)
		{
			if (!_templatesConfig.UserRegistration.EmailVerificationRequired)
				throw new InvalidOperationException("Email verification is not enabled.");

			User user;
			UserVerificationResult result = _credentialService.Verify(n, out user);
			if (result != UserVerificationResult.Verified)
				return View(new RegistrationPageVerifyViewModel(
					CurrentItem, false, result.GetDescription()));
			return View(new RegistrationPageVerifyViewModel(
				CurrentItem, false, null));
		}
	}
}