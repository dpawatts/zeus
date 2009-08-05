using System.Collections.Generic;

namespace Isis.Web.Security
{
	public interface ICredentialService
	{
		IUser CreateUser(string username, string password, string[] roles, out UserCreateStatus createStatus);
		IEnumerable<string> GetAllRoles();
		IEnumerable<IUser> GetAllUsers();
		IUser GetUser(string username);
		bool ValidateUser(string username, string password);
	}
}