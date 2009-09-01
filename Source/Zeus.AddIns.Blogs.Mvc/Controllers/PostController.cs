using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(Post), AreaName = BlogsWebPackage.AREA_NAME)]
	public class PostController : ZeusController<Post>
	{
		public override ActionResult Index()
		{
			return View(new PostViewModel(CurrentItem));
		}
	}
}