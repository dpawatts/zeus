using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(RecentPostsSummary), AreaName = BlogsWebPackage.AREA_NAME)]
	public class RecentPostsSummaryController : ZeusController<RecentPostsSummary>
	{
		public override ActionResult Index()
		{
			IEnumerable<Post> posts = Find.EnumerateAccessibleChildren(CurrentItem.Blog)
				//.Where(p => p.IsPublished())
				.OfType<Post>().OrderByDescending(p => p.Date);
			return PartialView(new RecentPostsSummaryViewModel(CurrentItem, posts));
		}
	}
}