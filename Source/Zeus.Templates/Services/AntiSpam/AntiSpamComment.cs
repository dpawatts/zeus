using System.Collections.Specialized;
using System.Net;
using Zeus.BaseLibrary.Web;

namespace Zeus.Templates.Services.AntiSpam
{
	public class AntiSpamComment : IAntiSpamComment
	{
		NameValueCollection _serverEnvironmentVariables;

		/// <summary>
		/// Initializes a new instance of the <see cref="AntiSpamComment"/> class.
		/// </summary>
		/// <param name="authorIPAddress">The author IP address.</param>
		/// <param name="authorUserAgent">The author user agent.</param>
		public AntiSpamComment(IPAddress authorIPAddress, string authorUserAgent)
		{
			IPAddress = authorIPAddress;
			UserAgent = authorUserAgent;
		}

		#region IAntiSpamComment Members

		/// <summary>
		/// The name submitted with the comment.
		/// </summary>
		public string Author { get; set; }

		/// <summary>
		/// The email submitted with the comment.
		/// </summary>
		public string AuthorEmail { get; set; }

		/// <summary>
		/// The url submitted if provided.
		/// </summary>
		public Url AuthorUrl { get; set; }

		/// <summary>
		/// Content of the comment
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// The HTTP_REFERER header value of the 
		/// originating comment.
		/// </summary>
		public string Referrer { get; set; }

		/// <summary>
		/// Permanent location of the entry the comment was 
		/// submitted to.
		/// </summary>
		public Url Permalink { get; set; }

		/// <summary>
		/// User agent of the requester. (Required)
		/// </summary>
		public string UserAgent { get; private set; }

		/// <summary>
		/// May be one of the following: {blank}, "comment", "trackback", "pingback", or a made-up value 
		/// like "registration".
		/// </summary>
		public string CommentType { get; set; }

		/// <summary>
		/// IPAddress of the submitter
		/// </summary>
		public IPAddress IPAddress { get; private set; }

		/// <summary>
		/// Optional collection of various server environment variables. 
		/// </summary>
		public NameValueCollection ServerEnvironmentVariables
		{
			get
			{
				_serverEnvironmentVariables = _serverEnvironmentVariables ?? new NameValueCollection();
				return _serverEnvironmentVariables;
			}
		}

		#endregion
	}
}