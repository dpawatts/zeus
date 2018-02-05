using System;
using System.Globalization;
using System.Web;
using Zeus.BaseLibrary.Web;
using Zeus.Net;
using Zeus.Templates.Configuration;

namespace Zeus.Templates.Services.AntiSpam
{
	/// <summary>
	/// The client class used to communicate with the 
	/// <see href="http://akismet.com/">Akismet</see> service.
	/// </summary>
	public class AkismetService : IAntiSpamService
	{
		private const string VERIFY_URL_FORMAT = "http://{0}/1.1/verify-key";
		private const string CHECK_URL_FORMAT = "http://{0}.{1}/1.1/comment-check";
		private const string SUBMIT_HAM_URL_FORMAT = "http://{0}.{1}/1.1/submit-ham";
		private const string SUBMIT_SPAM_URL_FORMAT = "http://{0}.{1}/1.1/submit-spam";
		private static readonly string _version = typeof(AkismetService).Assembly.GetName().Version.ToString();
		private readonly AkismetElement _configuration;
		private readonly IHttpClient _httpClient;
		private readonly Zeus.Web.IWebContext _webContext;
		private readonly Url _verifyUrl, _checkUrl, _submitHamUrl, _submitSpamUrl;
		private readonly string _userAgent;

		/// <summary>
		/// Initializes a new instance of the <see cref="AkismetService"/> class.
		/// </summary>
		public AkismetService(AkismetElement configuration, IHttpClient httpClient, Zeus.Web.IWebContext webContext)
		{
			_configuration = configuration;
			_httpClient = httpClient;
			_webContext = webContext;

			_verifyUrl = new Url(string.Format(CultureInfo.InvariantCulture, VERIFY_URL_FORMAT, _configuration.ApiBaseUrl));
			_checkUrl = new Url(string.Format(CultureInfo.InvariantCulture, CHECK_URL_FORMAT, _configuration.ApiKey, _configuration.ApiBaseUrl));
			_submitHamUrl = new Url(string.Format(CultureInfo.InvariantCulture, SUBMIT_HAM_URL_FORMAT, _configuration.ApiKey, _configuration.ApiBaseUrl));
			_submitSpamUrl = new Url(string.Format(CultureInfo.InvariantCulture, SUBMIT_SPAM_URL_FORMAT, _configuration.ApiKey, _configuration.ApiBaseUrl));

			_userAgent = BuildUserAgent("Zeus", _version);
		}

		/// <summary>
		/// Helper method for building a user agent string in the format 
		/// preferred by Akismet.
		/// </summary>
		/// <param name="applicationName">Name of the application.</param>
		/// <param name="appVersion">The version of the app.</param>
		/// <returns></returns>
		private static string BuildUserAgent(string applicationName, string appVersion)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}/{1} | Akismet/1.11", applicationName, appVersion);
		}

		/// <summary>
		/// Verifies the API key.  You really only need to
		/// call this once, perhaps at startup.
		/// </summary>
		/// <returns></returns>
		/// <exception type="Sytsem.Web.WebException">If it cannot make a request of Akismet.</exception>
		public bool VerifyApiKey(ContentItem currentStartPage)
		{
			string parameters = "key=" + HttpUtility.UrlEncode(_configuration.ApiKey) + "&blog="
				+ HttpUtility.UrlEncode(_webContext.GetFullyQualifiedUrl(currentStartPage.Url));
			string result = _httpClient.PostRequest(_verifyUrl, _userAgent, _configuration.Timeout, parameters);

			if (string.IsNullOrEmpty(result))
				throw new InvalidResponseException("Akismet returned an empty response");

			return string.Equals("valid", result, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Checks the comment and returns true if it is spam, otherwise false.
		/// </summary>
		/// <param name="currentStartPage"></param>
		/// <param name="comment"></param>
		/// <returns></returns>
		public bool CheckCommentForSpam(ContentItem currentStartPage, IAntiSpamComment comment)
		{
			string result = SubmitComment(currentStartPage, comment, _checkUrl);

			if (string.IsNullOrEmpty(result))
				throw new InvalidResponseException("Akismet returned an empty response");

			if (result != "true" && result != "false")
				throw new InvalidResponseException(string.Format(CultureInfo.InvariantCulture,
					"Received the response '{0}' from Akismet. Probably a bad API key.",
					result));

			return bool.Parse(result);
		}

		/// <summary>
		/// Submits a comment to Akismet that should have been 
		/// flagged as SPAM, but was not flagged by Akismet.
		/// </summary>
		/// <param name="currentStartPage"></param>
		/// <param name="comment"></param>
		/// <returns></returns>
		public virtual void SubmitSpam(ContentItem currentStartPage, IAntiSpamComment comment)
		{
			SubmitComment(currentStartPage, comment, _submitSpamUrl);
		}

		/// <summary>
		/// Submits a comment to Akismet that should not have been 
		/// flagged as SPAM (a false positive).
		/// </summary>
		/// <param name="currentStartPage"></param>
		/// <param name="comment"></param>
		/// <returns></returns>
		public void SubmitHam(ContentItem currentStartPage, IAntiSpamComment comment)
		{
			SubmitComment(currentStartPage, comment, _submitHamUrl);
		}

		string SubmitComment(ContentItem currentStartPage, IAntiSpamComment comment, Url url)
		{
			// Not too many concatenations.  Might not need a string builder.
			string parameters = "blog=" + HttpUtility.UrlEncode(_webContext.GetFullyQualifiedUrl(currentStartPage.Url))
				+ "&user_ip=" + comment.IPAddress
				+ "&user_agent=" + HttpUtility.UrlEncode(comment.UserAgent);

			if (!string.IsNullOrEmpty(comment.Referrer))
				parameters += "&referer=" + HttpUtility.UrlEncode(comment.Referrer);

			if (comment.Permalink != null)
				parameters += "&permalink=" + HttpUtility.UrlEncode(comment.Permalink.ToString());

			if (!string.IsNullOrEmpty(comment.CommentType))
				parameters += "&comment_type=" + HttpUtility.UrlEncode(comment.CommentType);

			if (!string.IsNullOrEmpty(comment.Author))
				parameters += "&comment_author=" + HttpUtility.UrlEncode(comment.Author);

			if (!string.IsNullOrEmpty(comment.AuthorEmail))
				parameters += "&comment_author_email=" + HttpUtility.UrlEncode(comment.AuthorEmail);

			if (comment.AuthorUrl != null)
				parameters += "&comment_author_url=" + HttpUtility.UrlEncode(comment.AuthorUrl.ToString());

			if (!string.IsNullOrEmpty(comment.Content))
				parameters += "&comment_content=" + HttpUtility.UrlEncode(comment.Content);

			if (comment.ServerEnvironmentVariables != null)
				foreach (string key in comment.ServerEnvironmentVariables)
					parameters += "&" + key + "=" + HttpUtility.UrlEncode(comment.ServerEnvironmentVariables[key]);

			return _httpClient.PostRequest(url, _userAgent, _configuration.Timeout, parameters).ToLowerInvariant();
		}
	}
}