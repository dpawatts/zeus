using System.Linq;
using System.Web.Mvc;
using Isis.Web;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.AddIns.Blogs.Services;
using Zeus.Templates.Mvc.Controllers;
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
			return View(new PostViewModel(CurrentItem, CurrentItem.GetChildren<FeedbackItem>()));
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Comment(string name, string url, string text)
		{
			_commentService.AddComment(CurrentItem, name, url, text);
			return Redirect(new Url(CurrentItem.Url).SetFragment(
				"comment" + CurrentItem.GetChildren<Comment>().Count()));
		}
	}
}