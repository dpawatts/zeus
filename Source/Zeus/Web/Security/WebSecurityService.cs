using System.Collections.Generic;
using System.Linq;
using Zeus.Security;

namespace Zeus.Web.Security
{
	public class WebSecurityService : IWebSecurityService
	{
		private readonly ICredentialService _credentialService;
		private readonly IAuthenticationContextService _authenticationContextService;

		public WebSecurityService(ICredentialService credentialService,
			IAuthenticationContextService authenticationContextService)
		{
			_credentialService = credentialService;
			_authenticationContextService = authenticationContextService;
		}

		public IEnumerable<User> GetRecentlyRegisteredUsers(int howMany)
		{
			return _credentialService.GetAllUsers()
				.Where(u => u.Verified)
				.OrderByDescending(u => u.Created)
				.Take(howMany);
		}

		public void SetAuthCookie(string userName, bool createPersistentCookie)
		{
			_authenticationContextService.GetCurrentService().SetAuthCookie(userName, createPersistentCookie);
		}

		public void SignOut()
		{
			_authenticationContextService.GetCurrentService().SignOut();
		}

		public bool ValidateUser(string username, string password)
		{
			return _credentialService.ValidateUser(username, password);
		}
	}
}