using System.Collections.Generic;
using Zeus.Security;

namespace Zeus.Web.Security
{
	public interface IWebSecurityService
	{
		IEnumerable<User> GetRecentlyRegisteredUsers(int howMany);
		void SetAuthCookie(string userName, bool createPersistentCookie);
		void SignOut();
		bool ValidateUser(string username, string password);
	}
}