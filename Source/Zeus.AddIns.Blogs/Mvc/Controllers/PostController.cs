using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.AddIns.Blogs.Services;
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

		public override ActionResult Index()
		{
			return View("Index", new PostViewModel(CurrentItem, _commentService.GetDisplayedComments(CurrentItem)));
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Comment(CommentFormViewModel commentForm)
		{
			if (!ModelState.IsValid)
				return Index();

			Comment comment;
			try
			{
				comment = _commentService.AddComment(CurrentItem, commentForm.Name, commentForm.Email, commentForm.Url, commentForm.Text);
			}
			catch (CaptchaException ex)
			{
				return View("Index", new PostViewModel(CurrentItem, _commentService.GetDisplayedComments(CurrentItem))
				{
					CaptchaError = ex.CaptchaError
				});
			}

			return Redirect(comment.Url);
		}
	}
}