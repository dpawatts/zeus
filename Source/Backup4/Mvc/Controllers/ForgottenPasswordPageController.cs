using System.Web.Mvc;
using Zeus.BaseLibrary.Web;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;
using Zeus.Web.Security;
using IWebContext = Zeus.Web.IWebContext;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(ForgottenPasswordPage), AreaName = TemplatesAreaRegistration.AREA_NAME)]
	public class ForgottenPasswordPageController : ZeusController<ForgottenPasswordPage>
	{
		private readonly ICredentialService _credentialService;
		private readonly IWebContext _webContext;

		public ForgottenPasswordPageController(ICredentialService credentialService, IWebContext webContext)
		{
			_credentialService = credentialService;
			_webContext = webContext;
		}

		public override ActionResult Index()
		{
			return View("Index", new ForgottenPasswordPageViewModel(CurrentItem));
		}

		[HttpPost]
		public ActionResult RequestReset(ForgottenPasswordPageRequestResetFormViewModel requestResetForm)
		{
			if (!ModelState.IsValid)
				return Index();

			PasswordResetRequestResult result = _credentialService.SendPasswordResetEmail(requestResetForm.Username,
				_webContext.GetFullyQualifiedUrl(new Url(CurrentItem.Url).AppendSegment("reset").AppendQuery("n=")),
				CurrentItem.PasswordResetEmailSender, CurrentItem.PasswordResetEmailSubject,
				CurrentItem.PasswordResetEmailBody);

			string status = null;
			switch (result)
			{
				case PasswordResetRequestResult.Sent :
					status = "To initiate the password reset process, please follow the instructions sent to your email address.";
					break;
				case PasswordResetRequestResult.RequestExists :
					status = "A password reset has already been requested for this username. Please check your email for the existing request, or alternatively try again later.";
					break;
				case PasswordResetRequestResult.TooManyRequests :
					status = "Too many password reset requests have been received for this username. To protect your security, the system will not allow any more requests at this time. Please try again later.";
					break;
				case PasswordResetRequestResult.UserNotFound :
					status = "No user matching the supplied username could be found.";
					break;
			}
			return View("RequestReset", new ForgottenPasswordPageRequestResetViewModel(CurrentItem, status));
		}

		[HttpGet]
		public ActionResult Reset(string n)
		{
			// Check that nonce exists.
			PasswordResetRequest resetRequest;
			bool valid = (_credentialService.CheckPasswordResetRequestValidity(n, out resetRequest) == PasswordResetRequestValidity.Valid);

			if (!valid)
				return View("ResetError", new ForgottenPasswordPageResetViewModel(CurrentItem));
			return View("Reset", new ForgottenPasswordPageResetViewModel(CurrentItem));
		}

		[HttpPost]
		public ActionResult Reset(string n, ForgottenPasswordPageResetFormViewModel resetForm)
		{
			// Get user record.
			if (!ModelState.IsValid)
				return Reset(n);

			// Reset password.
			PasswordResetResult result = _credentialService.ResetPassword(n, resetForm.Password);

			return View("ResetConfirmation", new ForgottenPasswordPageResetConfirmationViewModel(
				CurrentItem, result == PasswordResetResult.Succeeded));
		}
	}
}