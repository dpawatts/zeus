using System.Collections.Generic;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.AddIns.Blogs.Services;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(RecentPosts), AreaName = BlogsWebPackage.AREA_NAME)]
	public class RecentPostsController : ZeusController<RecentPosts>
	{
		private readonly IBlogService _blogService;

		public RecentPostsController(IBlogService blogService)
		{
			_blogService = blogService;
		}

		public override ActionResult Index()
		{
			IEnumerable<Post> posts = _blogService.GetRecentPosts(CurrentItem.Blog, CurrentItem.NumberToShow);
			return PartialView(new RecentPostsViewModel(CurrentItem, posts));
		}
	}
}