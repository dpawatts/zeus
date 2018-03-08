using System;
using System.Linq;
using System.Web.Mvc;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.BaseLibrary.Web;
using Zeus.Security;
using Zeus.Templates.Configuration;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Templates.Services;
using Zeus.Templates.Services.AntiSpam;
using Zeus.Web.Security;
using IWebContext = Zeus.Web.IWebContext;

namespace Zeus.Templates.Mvc.Controllers
{
	public abstract class RegistrationPageControllerBase<T, TFormViewModel> : ZeusController<T>
		where T : RegistrationPageBase
		where TFormViewModel : RegistrationPageFormViewModel
	{
		private readonly TemplatesSection _templatesConfig;
		private readonly IUserRegistrationService _userRegistrationService;
		private readonly ICaptchaService _captchaService;
		private readonly IWebContext _webContext;
		private readonly ICredentialService _credentialService;

		protected abstract string VerificationEmailSender { get; }
		protected abstract string VerificationEmailSubject { get; }
		protected abstract string VerificationEmailBody { get; }

		protected RegistrationPageControllerBase(TemplatesSection templatesConfig, IUserRegistrationService userRegistrationService,
			ICaptchaService captchaService, IWebContext webContext, ICredentialService credentialService)
		{
			_templatesConfig = templatesConfig;
			_userRegistrationService = userRegistrationService;
			_captchaService = captchaService;
			_webContext = webContext;
			_credentialService = credentialService;
		}

		public override ActionResult Index()
		{
            return View("Index", new RegistrationPageViewModel(CurrentItem));
		}

        [HttpPost]
        public virtual ActionResult Register(TFormViewModel registrationForm)
        {
            return Register(registrationForm, null);
        }

		[HttpPost]
		public virtual ActionResult Register(TFormViewModel registrationForm, DataContentItem membershipDetails)
		{
			if (!ModelState.IsValid)
				return Index();

			string captchaError;
			if (!_captchaService.Check(HttpContext, out captchaError))
				return View("Index", new RegistrationPageViewModel(CurrentItem) { CaptchaError = captchaError });

			// Create user.
			UserCreateStatus status = _userRegistrationService.CreateUser(registrationForm.Username,
				registrationForm.Password, registrationForm.Email,
				_webContext.GetFullyQualifiedUrl(new Url(CurrentItem.Url).AppendSegment("verify").AppendQuery("n=")),
				VerificationEmailSender, VerificationEmailSubject,
				VerificationEmailBody, registrationForm);

			if (status != UserCreateStatus.Success)
			{
				ModelState.AddModelError("RegistrationError", "Could not create user: " + status.GetDescription());
				return Index();
			}

            //add the profile info
            if (membershipDetails != null)
            {
                User NewUser = null;
                try
                {
                    NewUser = Zeus.Find.UserContainer().GetChildren<User>().Where(u => u.Username == registrationForm.Username.ToLower()).Single();
                }
                catch
                {
                    System.Web.HttpContext.Current.Response.Write("User Count = " + Zeus.Find.UserContainer().GetChildren<User>().Count());
                    System.Web.HttpContext.Current.Response.Write("Error: user not found = " + registrationForm.Username);
                    System.Web.HttpContext.Current.Response.End();
                }

                membershipDetails.AddTo(NewUser);
                Engine.Persister.Save(membershipDetails);
            }

			return View("RegisterConfirmation", new RegistrationPageConfirmationViewModel(
				CurrentItem, _templatesConfig.UserRegistration.EmailVerificationRequired,
				registrationForm.Email));
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
				CurrentItem, true, null));
		}
	}
}