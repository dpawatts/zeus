using System;
using Zeus.Web.Security;

namespace Zeus.Templates.Services
{
	public interface IUserRegistrationService
	{
		event EventHandler<UserRegisteredEventArgs> UserRegistered;

		UserCreateStatus CreateUser(string username, string password, string email,
			string verificationLinkRoot, string verificationEmailSender, string verificationEmailSubject,
			string verificationEmailBody, object registrationForm);
	}
}