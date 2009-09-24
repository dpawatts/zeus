using CookComputing.XmlRpc;
using Isis.Web;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Services;
using Zeus.AddIns.Blogs.Services.Tracking;
using Zeus.AddIns.Blogs.Web.XmlRpc;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Web.Pingback
{
	/// <summary>
	/// Service used to receive pingbacks from remote clients.
	/// </summary>
	public class PingbackXmlRpcService : ZeusXmlRpcService
	{
		private readonly IUrlParser _urlParser;
		private readonly ICommentService _commentService;

		public PingbackXmlRpcService(IUrlParser urlParser, ICommentService commentService)
		{
			_urlParser = urlParser;
			_commentService = commentService;
		}

		/// <summary>
		/// Method called by a remote client to ping this server.
		/// </summary>
		/// <param name="sourceURI">Source URI.</param>
		/// <param name="targetURI">Target URI.</param>
		/// <returns></returns>
		[XmlRpcMethod("pingback.ping", Description = "Pingback server implementation")]
		public string pingback(string sourceURI, string targetURI)
		{
			// Extract the post from targetURI.
			ContentItem testPost = _urlParser.Parse(targetURI);
			if (testPost == null)
				throw new XmlRpcFaultException(32, "The specified target URI does not exist.");

			Post post = testPost as Post;
			if (post == null)
				throw new XmlRpcFaultException(33, "The specified target URI cannot be used as a target.");

			if (!post.CurrentBlog.PingbacksEnabled)
				return "Pingbacks are not enabled for this site.";


			// Does the sourceURI actually contain the permalink?
			Url sourceUrl, targetUrl;
			Isis.Web.Url.TryParse(sourceURI, out sourceUrl);
			Isis.Web.Url.TryParse(targetURI, out targetUrl);

			string pageTitle;
			if (sourceUrl == null || targetUrl == null || !Verifier.SourceContainsTarget(sourceUrl, targetUrl, out pageTitle))
				throw new XmlRpcFaultException(17, "The source URI does not contain a link to the target URI, and so cannot be used as a source.");

			// Store pingback as a feedback item.
			_commentService.AddPingback(post, pageTitle, sourceUrl);
			
			return "Thanks for the pingback.";
		}
	}
}