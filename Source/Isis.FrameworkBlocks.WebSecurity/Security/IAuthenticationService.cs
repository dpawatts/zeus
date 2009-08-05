using System.Web;
using Isis.Web.Configuration;

namespace Isis.Web.Security
{
	public interface IAuthenticationService
	{
		bool Enabled { get; }
		AuthenticationLocation Config { get; }
		bool AccessingLoginPage();
		void RedirectFromLoginPage(string userName, bool createPersistentCookie);
		void SignOut();
		AuthenticationTicket Decrypt(string encryptedTicket);
		string Encrypt(AuthenticationTicket ticket);
		AuthenticationTicket ExtractTicketFromCookie();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ticket"></param>
		/// <param name="cookie">The existing cookie. Can be null if cookie is being created for the first time.</param>
		/// <returns></returns>
		void CreateOrUpdateCookieFromTicket(AuthenticationTicket ticket, HttpCookie cookie);

		AuthenticationTicket RenewTicketIfOld(AuthenticationTicket old);
		void SetAuthCookie(string userName, bool createPersistentCookie);
		string GetLoginPage(string extraQueryString, bool reuseReturnUrl);

		void RedirectToLoginPage(string extraQueryString);
		void RedirectToLoginPage();
	}
}