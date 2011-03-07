using System.Collections.Generic;
using Zeus.Security;

namespace Zeus.Web.Security
{
	public interface ICredentialService
	{
		User CreateUser(string username, string password, string email, string[] roles,
			bool isVerified, out UserCreateStatus createStatus);
		IEnumerable<string> GetAllRoles();
		IEnumerable<User> GetAllUsers();
		User GetUser(string username);
		bool ValidateUser(string username, string password);

		void SendVerificationEmail(User user, string linkRoot, string recipientEmail,
			string senderEmail, string emailSubject, string emailBody);

		UserVerificationResult Verify(string nonce, out User user);
		PasswordResetRequestValidity CheckPasswordResetRequestValidity(string nonce, out PasswordResetRequest resetRequest);
		PasswordResetResult ResetPassword(string nonce, string newPassword);

		PasswordResetRequestResult SendPasswordResetEmail(string username, string linkRoot,
			string senderEmail, string emailSubject, string emailBody);

		string EncryptPassword(string password);
	}
}