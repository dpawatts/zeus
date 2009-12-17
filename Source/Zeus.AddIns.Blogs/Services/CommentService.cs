using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.ContentTypes;
using Zeus.Persistence;
using Zeus.Templates.Services.AntiSpam;
using Zeus.Web;
using Zeus.Web.Mvc.Html;

namespace Zeus.AddIns.Blogs.Services
{
	public class CommentService : ICommentService
	{
		private const string CommentAuthorEmailCookieName = "CommentAuthorEmail";
		private const string CommentAuthorNameCookieName = "CommentAuthorName";

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

		public Comment AddComment(Post post, string name, string email, string url, string text)
		{
			Comment comment = _contentTypeManager.CreateInstance<Comment>(post);
			comment.AuthorName = name;
			comment.AuthorUrl = url;
			comment.AuthorEmail = email;
			comment.Text = StringExtensions.ConvertUrlsToHyperLinks(null, text);
			comment.AddTo(post);

			string error;
			if (!_captchaService.Check(new HttpContextWrapper(HttpContext.Current), out error))
				throw new CaptchaException(error, error);

			CheckForSpam(comment);

			SetCommonFeedbackItemProperties(post, comment);

			// Set user cookie so we know they were the author - this is not
			// particularly secure, but it's how WordPress does it.
			_webContext.Response.Cookies.Set(new HttpCookie(CommentAuthorEmailCookieName, email));
			_webContext.Response.Cookies.Set(new HttpCookie(CommentAuthorNameCookieName, name));

			_persister.Save(comment);

			return comment;
		}

		public void AddPingback(Post post, string sourcePageTitle, string sourceUrl)
		{
			Pingback comment = _contentTypeManager.CreateInstance<Pingback>(post);
			comment.SourcePageTitle = sourcePageTitle;
			comment.SourceUrl = sourceUrl;
			comment.AddTo(post);

			CheckForSpam(comment);

			SetCommonFeedbackItemProperties(post, comment);

			_persister.Save(comment);
		}

		public IEnumerable<FeedbackItem> GetDisplayedComments(Post post)
		{
			HttpCookie authorEmailCookie = _webContext.Request.Cookies[CommentAuthorEmailCookieName];
			string authorEmail = (authorEmailCookie != null) ? authorEmailCookie.Value : null;

			HttpCookie authorNameCookie = _webContext.Request.Cookies[CommentAuthorNameCookieName];
			string authorName = (authorNameCookie != null) ? authorNameCookie.Value : null;

			return post.Comments.Where(fi => fi.Approved || ((fi is Comment) && ((Comment) fi).AuthorEmail == authorEmail && ((Comment) fi).AuthorName == authorName));
		}

		private static void SetCommonFeedbackItemProperties(Post post, FeedbackItem feedbackItem)
		{
			feedbackItem.Number = post.GetChildren<FeedbackItem>().Max(fi => fi.Number) + 1;
			feedbackItem.Title = "Feedback Item #" + feedbackItem.Number;
			feedbackItem.Approved = !post.CurrentBlog.CommentModerationEnabled && !feedbackItem.Spam;
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