using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(Blog), AreaName = BlogsWebPackage.AREA_NAME)]
	public class BlogController : ZeusController<Blog>
	{
		public override ActionResult Index()
		{
			IEnumerable<Post> recentPosts = Find.EnumerateAccessibleChildren(CurrentItem)
				//.Where(p => p.IsPublished())
				.OfType<Post>().OrderByDescending(p => p.Date);
			return View(new BlogViewModel(CurrentItem, recentPosts));
		}
	}
}