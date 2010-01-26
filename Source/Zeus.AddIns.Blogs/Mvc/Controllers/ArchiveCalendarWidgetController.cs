using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.Linq;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(ArchiveCalendarWidget), AreaName = BlogsAreaRegistration.AREA_NAME)]
	public class ArchiveCalendarWidgetController : WidgetController<ArchiveCalendarWidget>
	{
		public override ActionResult Index()
		{
			return PartialView(new ArchiveCalendarWidgetViewModel(CurrentItem));
		}

		public override ActionResult Header()
		{
			return PartialView(new ArchiveCalendarWidgetViewModel(CurrentItem));
		}

		public JsonResult GetData(int month, int year)
		{
			++month;
			IEnumerable<Post> posts = Find.EnumerateAccessibleChildren(CurrentItem.Blog)
				.NavigablePages().OfType<Post>()
				.Where(post => post.Date.Month == month && post.Date.Year == year)
				.OrderBy(post => post.Date);

			return Json(new { dates = posts.Select(post => new
				{
					date = post.Date.Day, url = post.Url
				})
			}, JsonRequestBehavior.AllowGet);
		}
	}
}