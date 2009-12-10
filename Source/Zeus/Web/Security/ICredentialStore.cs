using System.Collections.Generic;

namespace Zeus.Web.Security
{
	public interface ICredentialStore
	{
		void CreateUser(string username, string password, string[] roles, bool verified);
		IEnumerable<string> GetAllRoles();
		IEnumerable<IUser> GetAllUsers();
		IUser GetUser(string username);
		IUser GetUserByNonce(string nonce);
		void SaveNonce(IUser user, string nonce);
		void VerifyUser(IUser user);
	}
}