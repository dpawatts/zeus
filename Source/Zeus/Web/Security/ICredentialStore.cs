using System.Collections.Generic;
using Zeus.Security;

namespace Zeus.Web.Security
{
	public interface ICredentialStore
	{
		void CreateUser(string username, string password, string[] roles, bool verified);
		IEnumerable<string> GetAllRoles();
		IEnumerable<User> GetAllUsers();
		User GetUser(string username);
		User GetUserByNonce(string nonce);
		void SaveNonce(User user, string nonce);
		void VerifyUser(User user);
		PasswordResetRequest GetPasswordResetRequestByNonce(string nonce);
		User GetUserByEmail(string email);
	}
}