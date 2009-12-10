using System.Collections.Generic;

namespace Zeus.Web.Security
{
	public interface ICredentialService
	{
		IUser CreateUser(string username, string password, string email, string[] roles,
			bool isVerified, out UserCreateStatus createStatus);
		IEnumerable<string> GetAllRoles();
		IEnumerable<IUser> GetAllUsers();
		IUser GetUser(string username);
		bool ValidateUser(string username, string password);

		void SendVerificationEmail(IUser user, string linkRoot, string recipientEmail,
			string senderEmail, string emailSubject, string emailBody);

		UserVerificationResult Verify(string nonce, out IUser user);
	}
}