using System;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.AddIns.Blogs.Services;
using Zeus.BaseLibrary.Web;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Templates.Services.AntiSpam;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(Post), AreaName = BlogsWebPackage.AREA_NAME)]
	public class PostController : ZeusController<Post>
	{
		private readonly ICommentService _commentService;

		public PostController(ICommentService commentService)
		{
			_commentService = commentService;
		}

		[NonAction]
		public override ActionResult Index()
		{
			throw new NotSupportedException();
		}

		public ActionResult Index(string captchaError)
		{
			return View(new PostViewModel(CurrentItem, CurrentItem.GetChildren<FeedbackItem>())
			{
				CaptchaError = captchaError
			});
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Comment(string name, string url, string text)
		{
			try
			{
				_commentService.AddComment(CurrentItem, name, url, text);
			}
			catch (CaptchaException ex)
			{
				return Redirect(new Url(CurrentItem.Url).Query("captchaError", ex.CaptchaError));
			}
			return Redirect(new Url(CurrentItem.Url).SetFragment(
				"comment" + CurrentItem.GetChildren<Comment>().Count()));
		}
	}
}