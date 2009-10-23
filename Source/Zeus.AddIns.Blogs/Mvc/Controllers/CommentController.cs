using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(Comment), AreaName = BlogsWebPackage.AREA_NAME)]
	public class CommentController : ZeusController<Comment>
	{
		public override ActionResult Index()
		{
			return PartialView(new CommentViewModel(CurrentItem));
		}
	}
}