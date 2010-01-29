using System;
using Zeus.Security;
using Zeus.Templates.Configuration;
using Zeus.Web.Security;

namespace Zeus.Templates.Services
{
	public class UserRegistrationService : IUserRegistrationService
	{
		private readonly TemplatesSection _templatesConfig;
		private readonly ICredentialService _credentialService;

		public event EventHandler<UserRegisteredEventArgs> UserRegistered;

		#region Constructors

		public UserRegistrationService(TemplatesSection templatesConfig, ICredentialService credentialService)
		{
			_templatesConfig = templatesConfig;
			_credentialService = credentialService;
		}

		#endregion

		public UserCreateStatus CreateUser(string username, string password, string email,
			string verificationLinkRoot, string verificationEmailSender, string verificationEmailSubject,
			string verificationEmailBody, object registrationForm)
		{
			// Create user.
			UserCreateStatus status;
			User user = _credentialService.CreateUser(username, password, email,
				new[] { _templatesConfig.UserRegistration.DefaultRole },
				!_templatesConfig.UserRegistration.EmailVerificationRequired,
				out status);
			if (status != UserCreateStatus.Success)
				return status;

			// Allow other websites to create profile.
			if (UserRegistered != null)
				UserRegistered(this, new UserRegisteredEventArgs(user, registrationForm));

			// Send verification email.
			if (_templatesConfig.UserRegistration.EmailVerificationRequired)
				_credentialService.SendVerificationEmail(user,
					verificationLinkRoot, email,
					verificationEmailSender,
					verificationEmailSubject,
					verificationEmailBody);

			return status;
		}
	}

	public class UserRegisteredEventArgs : EventArgs
	{
		public User User { get; private set; }
		public object RegistrationForm { get; set; }

		public UserRegisteredEventArgs(User user, object registrationForm)
		{
			User = user;
			RegistrationForm = registrationForm;
		}
	}
}