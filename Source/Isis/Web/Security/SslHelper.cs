using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Isis.Web.Configuration;

namespace Isis.Web.Security
{
	/// <summary>
	/// Provides static methods for ensuring that a page is rendered 
	/// securely via SSL or unsecurely.
	/// </summary>
	public sealed class SslHelper
	{

		// Protocol prefixes.
		private const string UnsecureProtocolPrefix = "http://";
		private const string SecureProtocolPrefix = "https://";

		/// <summary>
		/// Prevent creating an instance of this class.
		/// </summary>
		private SslHelper()
		{
		}

		/// <summary>
		/// Determines the secure page that should be requested if a redirect occurs.
		/// </summary>
		/// <param name="settings">The SecureWebPageSettings to use in determining.</param>
		/// <param name="ignoreCurrentProtocol">
		/// A flag indicating whether or not to ingore the current protocol when determining.
		/// </param>
		/// <returns>A string containing the absolute URL of the secure page to redirect to.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static string DetermineSecurePage(SecureWebPagesSection settings, bool ignoreCurrentProtocol)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");

			string Result = null;
			HttpRequest Request = HttpContext.Current.Request;

			// Is this request already secure?
			string RequestPath = Request.Url.AbsoluteUri;
			if (ignoreCurrentProtocol || RequestPath.StartsWith(UnsecureProtocolPrefix))
			{
				// Is there a different URI to redirect to?
				if (string.IsNullOrEmpty(settings.EncryptedUri))
					// Replace the protocol of the requested URL with "https".
					// * Account for cookieless sessions by applying the application modifier.
					Result = string.Concat(
						SecureProtocolPrefix,
						Request.Url.Authority,
						HttpContext.Current.Response.ApplyAppPathModifier(Request.RawUrl),
						Request.Url.Query
					);
				else
					// Build the URL with the "https" protocol.
					Result = BuildUrl(true, settings.MaintainPath, settings.EncryptedUri, settings.UnencryptedUri);
			}

			return Result;
		}

		/// <summary>
		/// Determines the unsecure page that should be requested if a redirect occurs.
		/// </summary>
		/// <param name="settings">The SecureWebPageSettings to use in determining.</param>
		/// <param name="ignoreCurrentProtocol">
		/// A flag indicating whether or not to ingore the current protocol when determining.
		/// </param>
		/// <returns>A string containing the absolute URL of the unsecure page to redirect to.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static string DetermineUnsecurePage(SecureWebPagesSection settings, bool ignoreCurrentProtocol)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");

			string Result = null;
			HttpRequest Request = HttpContext.Current.Request;

			// Is this request secure?
			string RequestPath = Request.Url.AbsoluteUri;
			if (ignoreCurrentProtocol || RequestPath.StartsWith(SecureProtocolPrefix))
			{
				// Is there a different URI to redirect to?
				if (string.IsNullOrEmpty(settings.UnencryptedUri))
					// Replace the protocol of the requested URL with "http".
					// * Account for cookieless sessions by applying the application modifier.
					Result = string.Concat(
						UnsecureProtocolPrefix,
						Request.Url.Authority,
						HttpContext.Current.Response.ApplyAppPathModifier(Request.RawUrl),
						Request.Url.Query
					);
				else
					// Build the URL with the "http" protocol.
					Result = BuildUrl(false, settings.MaintainPath, settings.EncryptedUri, settings.UnencryptedUri);
			}

			return Result;
		}

		/// <summary>
		/// Requests the current page over a secure connection, if it is not already.
		/// </summary>
		/// <param name="settings">The SecureWebPageSettings to use for this request.</param>
		public static void RequestSecurePage(SecureWebPagesSection settings)
		{
			// Determine the response path, if any.
			string ResponsePath = DetermineSecurePage(settings, false);
			if (!string.IsNullOrEmpty(ResponsePath))
				// Redirect to the secure page.
				HttpContext.Current.Response.Redirect(ResponsePath, true);
		}

		/// <summary>
		/// Requests the current page over an unsecure connection, if it is not already.
		/// </summary>
		/// <param name="settings">The SecureWebPageSettings to use for this request.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public static void RequestUnsecurePage(SecureWebPagesSection settings)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");

			// Determine the response path, if any.
			string ResponsePath = DetermineUnsecurePage(settings, false);
			if (!string.IsNullOrEmpty(ResponsePath))
			{
				HttpRequest Request = HttpContext.Current.Request;

				// Test for the need to bypass a security warning.
				bool Bypass;
				if (settings.WarningBypassMode == SecurityWarningBypassMode.AlwaysBypass)
					Bypass = true;
				else if (settings.WarningBypassMode == SecurityWarningBypassMode.BypassWithQueryParam &&
						Request.QueryString[settings.BypassQueryParamName] != null)
				{
					Bypass = true;

					// Remove the bypass query parameter from the URL.
					System.Text.StringBuilder NewPath = new System.Text.StringBuilder(ResponsePath);
					int i = ResponsePath.LastIndexOf(string.Format("?{0}=", settings.BypassQueryParamName));
					if (i < 0)
						i = ResponsePath.LastIndexOf(string.Format("&{0}=", settings.BypassQueryParamName));
					NewPath.Remove(i, settings.BypassQueryParamName.Length + Request.QueryString[settings.BypassQueryParamName].Length + 1);

					// Remove any abandoned "&" character.
					if (i >= NewPath.Length)
						i = NewPath.Length - 1;
					if (NewPath[i] == '&')
						NewPath.Remove(i, 1);

					// Remove any abandoned "?" character.
					i = NewPath.Length - 1;
					if (NewPath[i] == '?')
						NewPath.Remove(i, 1);

					ResponsePath = NewPath.ToString();
				}
				else
					Bypass = false;

				// Output a redirector for the needed page to avoid a security warning.
				HttpResponse Response = HttpContext.Current.Response;
				if (Bypass)
				{
					// Clear the current response.
					Response.Clear();

					// Add a refresh header to the response for the new path.
					Response.AddHeader("Refresh", string.Concat("0;URL=", ResponsePath));

					// Also, add JavaScript to replace the current location as backup.
					Response.Write("<html><head><title></title>");
					Response.Write("<!-- <script language=\"javascript\">window.location.replace(\"");
					Response.Write(ResponsePath);
					Response.Write("\");</script> -->");
					Response.Write("</head><body></body></html>");

					Response.End();
				}
				else
					// Redirect to the unsecure page.
					Response.Redirect(ResponsePath, true);
			}
		}

		/// <summary>
		/// Builds a URL from the given protocol and appropriate host path. The resulting URL 
		/// will maintain the current path if requested.
		/// </summary>
		/// <param name="secure">Is this to be a secure URL?</param>
		/// <param name="maintainPath">Should the current path be maintained during transfer?</param>
		/// <param name="encryptedUri">The URI to redirect to for encrypted requests.</param>
		/// <param name="unencryptedUri">The URI to redirect to for standard requests.</param>
		/// <returns></returns>
		private static string BuildUrl(bool secure, bool maintainPath, string encryptedUri, string unencryptedUri)
		{
			// Clean the URIs.
			encryptedUri = CleanHostUri(string.IsNullOrEmpty(encryptedUri) ? unencryptedUri : encryptedUri);
			unencryptedUri = CleanHostUri(string.IsNullOrEmpty(unencryptedUri) ? encryptedUri : unencryptedUri);

			// Get the current request.
			HttpRequest Request = HttpContext.Current.Request;

			// Prepare to build the needed URL.
			System.Text.StringBuilder Url = new System.Text.StringBuilder();

			// Host authority (e.g. secure.mysite.com/).
			if (secure)
				Url.Append(encryptedUri);
			else
				Url.Append(unencryptedUri);

			if (maintainPath)
				// Append the current file path.
				Url.Append(Request.RawUrl);
			else
			{
				// Append just the current page
				string CurrentUrl = Request.RawUrl;
				Url.Append(CurrentUrl.Substring(CurrentUrl.LastIndexOf('/') + 1)).Append(Request.Url.Query);
			}

			// Replace any double slashes with a single slash.
			Url.Replace("//", "/");

			// Prepend the protocol.
			if (secure)
				Url.Insert(0, SecureProtocolPrefix);
			else
				Url.Insert(0, UnsecureProtocolPrefix);

			return Url.ToString();
		}

		/// <summary>
		/// Cleans a host path by stripping out any unneeded elements.
		/// </summary>
		/// <param name="uri">The host URI to validate.</param>
		/// <returns>Returns a string that is stripped as needed.</returns>
		private static string CleanHostUri(string uri)
		{
			string Result = string.Empty;
			if (!string.IsNullOrEmpty(uri))
			{
				// Ensure there is a protocol or a Uri cannot be constructed.
				if (!uri.StartsWith(UnsecureProtocolPrefix) && !uri.StartsWith(SecureProtocolPrefix))
					uri = UnsecureProtocolPrefix + uri;

				// Extract the authority and path to build a string suitable for our needs.
				Uri HostUri = new Uri(uri);
				Result = string.Concat(HostUri.Authority, HostUri.AbsolutePath);
				if (!Result.EndsWith("/"))
					Result += "/";
			}

			return Result;
		}

	}
}
