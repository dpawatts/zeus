using System.Collections.Generic;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.AddIns.Blogs.Services;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(RecentPostsSummary), AreaName = BlogsAreaRegistration.AREA_NAME)]
	public class RecentPostsSummaryController : WidgetController<RecentPostsSummary>
	{
		private readonly IBlogService _blogService;

		public RecentPostsSummaryController(IBlogService blogService)
		{
			_blogService = blogService;
		}

		public override ActionResult Index()
		{
			IEnumerable<Post> posts = _blogService.GetRecentPosts(CurrentItem.Blog, CurrentItem.NumberToShow);
			return PartialView(new RecentPostsSummaryViewModel(CurrentItem, posts));
		}
	}
}