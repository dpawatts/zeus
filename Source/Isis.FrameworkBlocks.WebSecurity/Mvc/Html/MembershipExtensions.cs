using System;
using System.Web.Mvc;
using Isis.Web.Security;

namespace Isis.Web.Mvc.Html
{
	public static class MembershipExtensions
	{
		private static IAuthenticationService CurrentAuthenticationService
		{
			get { return WebSecurityEngine.Get<IAuthenticationContextService>().GetCurrentService(); }
		}

		public static string CurrentUsername(this HtmlHelper html, string formatString)
		{
			string username = GetUsername(html);
			if (string.IsNullOrEmpty(username))
				return string.Empty;

			if (formatString.Length == 0)
				return username;

			try
			{
				return string.Format(formatString, username);
			}
			catch (FormatException exception)
			{
				throw new FormatException("FormatString is not a valid format string.", exception);
			}
		}

		public static string LoginUrl(this HtmlHelper html)
		{
			return CurrentAuthenticationService.GetLoginPage(null, true);
		}

		public static bool IsUserLoggedIn(this HtmlHelper html)
		{
			return (html.ViewContext.HttpContext.User != null && html.ViewContext.HttpContext.User.Identity != null && html.ViewContext.HttpContext.User.Identity.IsAuthenticated);
		}

		private static string GetUsername(HtmlHelper html)
		{
			return (IsUserLoggedIn(html)) ? html.ViewContext.HttpContext.User.Identity.Name : null;
		}
	}
}