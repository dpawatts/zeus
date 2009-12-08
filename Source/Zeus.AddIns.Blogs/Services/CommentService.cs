using System.Net;
using System.Web;
using Zeus.AddIns.AntiSpam;
using Zeus.AddIns.AntiSpam.Services;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.ContentTypes;
using Zeus.Persistence;
using Zeus.Web;
using Zeus.Web.Mvc.Html;

namespace Zeus.AddIns.Blogs.Services
{
	public class CommentService : ICommentService
	{
		private readonly IContentTypeManager _contentTypeManager;
		private readonly IPersister _persister;
		private readonly ICaptchaService _captchaService;
		private readonly IAntiSpamService _antiSpamService;
		private readonly IWebContext _webContext;

		public CommentService(IContentTypeManager contentTypeManager, IPersister persister,
			ICaptchaService captchaService, IAntiSpamService antiSpamService,
			IWebContext webContext)
		{
			_contentTypeManager = contentTypeManager;
			_persister = persister;
			_captchaService = captchaService;
			_antiSpamService = antiSpamService;
			_webContext = webContext;
		}

		public void AddComment(Post post, string name, string url, string text)
		{
			Comment comment = _contentTypeManager.CreateInstance<Comment>(post);
			comment.AuthorName = name;
			comment.AuthorUrl = url;
			comment.Text = StringExtensions.ConvertUrlsToHyperLinks(null, text);
			comment.AddTo(post);

			_captchaService.Check(new HttpContextWrapper(HttpContext.Current));

			CheckForSpam(comment);

			_persister.Save(comment);
		}

		public void AddPingback(Post post, string sourcePageTitle, string sourceUrl)
		{
			Pingback comment = _contentTypeManager.CreateInstance<Pingback>(post);
			comment.SourcePageTitle = sourcePageTitle;
			comment.SourceUrl = sourceUrl;
			comment.AddTo(post);

			CheckForSpam(comment);

			_persister.Save(comment);
		}

		private void CheckForSpam(FeedbackItem feedbackItem)
		{
			AntiSpamComment antiSpamComment = ConvertToAntiSpamComment(feedbackItem);
			feedbackItem.Spam = _antiSpamService.CheckCommentForSpam(Find.StartPage, antiSpamComment);
		}

		private AntiSpamComment ConvertToAntiSpamComment(FeedbackItem feedbackItem)
		{
			AntiSpamComment antiSpamComment = new AntiSpamComment(IPAddress.Parse(_webContext.Request.UserHostAddress), _webContext.Request.UserAgent)
			{
				Author = feedbackItem.AntiSpamAuthorName,
				AuthorEmail = feedbackItem.AntiSpamAuthorEmail,
				AuthorUrl = feedbackItem.AntiSpamAuthorUrl,
				CommentType = feedbackItem.AntiSpamFeedbackType,
				Content = feedbackItem.AntiSpamContent,
				Permalink = _webContext.GetFullyQualifiedUrl(feedbackItem.Url),
				Referrer = string.Empty
			};
			return antiSpamComment;
		}
	}
}