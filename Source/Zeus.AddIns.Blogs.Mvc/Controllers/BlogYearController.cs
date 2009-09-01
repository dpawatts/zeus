using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(BlogYear), AreaName = BlogsWebPackage.AREA_NAME)]
	public class BlogYearController : ZeusController<BlogYear>
	{
		public override ActionResult Index()
		{
			IEnumerable<Post> posts = Find.EnumerateAccessibleChildren(CurrentItem)
				//.Where(p => p.IsPublished())
				.OfType<Post>().OrderByDescending(p => p.Date);
			return View(new BlogYearViewModel(CurrentItem, CurrentItem.Year, posts));
		}
	}
}