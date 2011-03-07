using System.Web;
using System.Web.Security;

namespace Zeus.Web.Security
{
	public interface IAuthenticationService
	{
		bool Enabled { get; }
		AuthenticationLocation Config { get; }
		string LoginUrl { get; }

		bool AccessingLoginPage();
		void RedirectFromLoginPage(string userName, bool createPersistentCookie);
		void SignOut();
		FormsAuthenticationTicket Decrypt(string encryptedTicket);
		string Encrypt(FormsAuthenticationTicket ticket);
		FormsAuthenticationTicket ExtractTicketFromCookie();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ticket"></param>
		/// <param name="cookie">The existing cookie. Can be null if cookie is being created for the first time.</param>
		/// <returns></returns>
		void CreateOrUpdateCookieFromTicket(FormsAuthenticationTicket ticket, HttpCookie cookie);

		FormsAuthenticationTicket RenewTicketIfOld(FormsAuthenticationTicket old);
		void SetAuthCookie(string userName, bool createPersistentCookie);
		string GetLoginPage(string extraQueryString, bool reuseReturnUrl);

		void RedirectToLoginPage(string extraQueryString);
		void RedirectToLoginPage();
	}
}