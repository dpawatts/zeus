using System.Web;
using System;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.BaseLibrary.ExtensionMethods.Collections;
using Zeus.BaseLibrary.Web;
using Zeus.Configuration;
using Zeus.Properties;
using System.Web.Security;

namespace Zeus.Web.Security
{
	public class AuthenticationService : IAuthenticationService
	{
		#region Fields

		private readonly BaseLibrary.Web.IWebContext _webContext;
		private readonly AuthenticationLocation _config;
		private readonly string _loginUrl;

		#endregion

		#region Constructor

		public AuthenticationService(BaseLibrary.Web.IWebContext webContext, AuthenticationLocation config, string loginUrl)
		{
			_webContext = webContext;
			_config = config;
			_loginUrl = loginUrl;
		}

		#endregion

		#region Properties

		public bool Enabled
		{
			get { return _config != null && _config.Enabled; }
		}

		public AuthenticationLocation Config
		{
			get { return _config; }
		}

		public string LoginUrl
		{
			get { return _loginUrl; }
		}

		#endregion

		#region Methods

		public bool AccessingLoginPage()
		{
			string loginUrl = (!string.IsNullOrEmpty(_loginUrl) && VirtualPathUtility.IsAppRelative(_loginUrl)) ? VirtualPathUtility.ToAbsolute(_loginUrl) : _loginUrl;
			return _webContext.Request.Path.Equals(loginUrl, StringComparison.InvariantCultureIgnoreCase);
		}

		public void RedirectFromLoginPage(string userName, bool createPersistentCookie)
		{
			if (userName == null)
				throw new ArgumentException("userName must be set", "userName");

			string returnUrl = GetReturnUrl(true);
			SetAuthCookie(userName, createPersistentCookie);
			_webContext.Response.Redirect(returnUrl, false);
		}

		public string GetLoginPage(string extraQueryString, bool reuseReturnUrl)
		{
			HttpContext current = HttpContext.Current;
			string loginUrl = _loginUrl;
			if (loginUrl.IndexOf('?') >= 0)
			{
				loginUrl = new Url(loginUrl).SetQueryParameter("ReturnUrl", null).ToString();
			}
			int index = loginUrl.IndexOf('?');
			if (index < 0)
			{
				loginUrl = loginUrl + "?";
			}
			else if (index < (loginUrl.Length - 1))
			{
				loginUrl = loginUrl + "&";
			}
			string str2 = null;
			if (reuseReturnUrl)
			{
				str2 = HttpUtility.UrlEncode(GetReturnUrl(false), current.Request.ContentEncoding);
			}
			if (str2 == null)
			{
				str2 = HttpUtility.UrlEncode(_webContext.Url.PathAndQuery, current.Request.ContentEncoding);
			}
			loginUrl = loginUrl + "ReturnUrl=" + str2;
			if (!string.IsNullOrEmpty(extraQueryString))
			{
				loginUrl = loginUrl + "&" + extraQueryString;
			}
			return loginUrl;
		}

		private string GetLoginPage(string extraQueryString)
		{
			return GetLoginPage(extraQueryString, false);
		}

		public void RedirectToLoginPage(string extraQueryString)
		{
			string loginPage = GetLoginPage(extraQueryString);
			_webContext.Response.Redirect(loginPage, false);
		}

		public void RedirectToLoginPage()
		{
			RedirectToLoginPage(null);
		}

		private string GetReturnUrl(bool useDefaultIfAbsent)
		{
			string str = _webContext.Request.QueryString["ReturnUrl"];
			if ((!string.IsNullOrEmpty(str) && !str.Contains("/")) && str.Contains("%"))
				str = HttpUtility.UrlDecode(str);
			if (string.IsNullOrEmpty(str) && useDefaultIfAbsent)
				return _config.DefaultUrl;
			return str;
		}

		public void SignOut()
		{
			string str = string.Empty;
			if (_webContext.Request.Browser["supportsEmptyStringInCookieValue"] == "false")
				str = "NoCookie";
			HttpCookie cookie = new HttpCookie(_config.Name, str)
			{
				HttpOnly = true,
				Path = _config.CookiePath,
				Expires = new DateTime(0x7cf, 10, 12),
				Secure = _config.RequireSsl
			};
			if (_config.CookieDomain != null)
				cookie.Domain = _config.CookieDomain;
			_webContext.Response.Cookies.Remove(_config.Name);
			_webContext.Response.Cookies.Add(cookie);
		}

		#region AuthCookie methods

		public FormsAuthenticationTicket Decrypt(string encryptedTicket)
		{
			return FormsAuthentication.Decrypt(encryptedTicket);
		}

		public string Encrypt(FormsAuthenticationTicket ticket)
		{
			return FormsAuthentication.Encrypt(ticket);
		}

		public FormsAuthenticationTicket ExtractTicketFromCookie()
		{
			FormsAuthenticationTicket ticket = null;
			string encryptedTicket = null;
			HttpCookie cookie = _webContext.Request.Cookies[_config.Name];
			if (cookie != null)
				encryptedTicket = cookie.Value;

			if (!string.IsNullOrEmpty(encryptedTicket))
			{
				try
				{
					ticket = Decrypt(encryptedTicket);
				}
				catch
				{
					_webContext.Request.Cookies.Remove(_config.Name);
				}

				if (ticket != null && !ticket.Expired && (!_config.RequireSsl || _webContext.Request.IsSecureConnection))
					return ticket;

				_webContext.Request.Cookies.Remove(_config.Name);
			}

			return null;
		}

		private void CreateAuthCookie(string userName, bool createPersistentCookie)
		{
			if (userName == null)
				userName = string.Empty;

			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName,
				DateTime.Now, DateTime.Now.AddMinutes(_config.Timeout.TotalMinutes),
				createPersistentCookie, string.Empty, _config.CookiePath);

			CreateOrUpdateCookieFromTicket(ticket, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ticket"></param>
		/// <param name="cookie">The existing cookie. Can be null if cookie is being created for the first time.</param>
		/// <returns></returns>
		public void CreateOrUpdateCookieFromTicket(FormsAuthenticationTicket ticket, HttpCookie cookie)
		{
			string cookieValue = Encrypt(ticket);
			if (string.IsNullOrEmpty(cookieValue))
				throw new HttpException(Resources.WebSecurityUnableToEncryptCookieTicket);

			if (cookie == null)
				cookie = new HttpCookie(_config.Name);

			if (ticket.IsPersistent)
				cookie.Expires = ticket.Expiration;
			cookie.Path = ticket.CookiePath;
			cookie.Value = cookieValue;
			cookie.Secure = _config.RequireSsl;
			cookie.HttpOnly = true;
			if (_config.CookieDomain != null)
				cookie.Domain = _config.CookieDomain;

			_webContext.Response.Cookies.Remove(cookie.Name);
			_webContext.Response.Cookies.Add(cookie);
		}

		public FormsAuthenticationTicket RenewTicketIfOld(FormsAuthenticationTicket old)
		{
			if (old == null)
				return null;

			DateTime now = DateTime.Now;
			TimeSpan span = now - old.IssueDate;
			TimeSpan span2 = old.Expiration - now;
			if (span2 > span)
				return old;

			return new FormsAuthenticationTicket(old.Version, old.Name, now, now + (old.Expiration - old.IssueDate), old.IsPersistent, old.UserData, old.CookiePath);
		}

		public void SetAuthCookie(string userName, bool createPersistentCookie)
		{
			if (!_webContext.Request.IsSecureConnection && _config.RequireSsl)
				throw new HttpException(Resources.WebSecurityConnectionNotSecureCreatingSecureCookie);

			CreateAuthCookie(userName, createPersistentCookie);
		}

		#endregion

		#endregion
	}
}