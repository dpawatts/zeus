using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(RecentPosts), AreaName = BlogsWebPackage.AREA_NAME)]
	public class RecentPostsController : ZeusController<RecentPosts>
	{
		public override ActionResult Index()
		{
			IEnumerable<Post> posts = Find.EnumerateAccessibleChildren(CurrentItem.Blog)
				//.Where(p => p.IsPublished())
				.OfType<Post>().OrderByDescending(p => p.Date)
				.Take(CurrentItem.NumberToShow);
			return PartialView(new RecentPostsViewModel(CurrentItem, posts));
		}
	}
}