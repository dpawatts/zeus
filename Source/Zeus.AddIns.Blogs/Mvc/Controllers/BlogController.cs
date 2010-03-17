using System;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.Linq;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;
using System.Collections.Generic;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(Blog), AreaName = BlogsAreaRegistration.AREA_NAME)]
	public class BlogController : ZeusController<Blog>
	{
		[NonAction]
		public override ActionResult Index()
		{
			throw new InvalidOperationException();
		}

		public ActionResult Index(int? p)
		{
			IPageable<Post> recentPosts = Find.EnumerateAccessibleChildren(CurrentItem)
				.NavigablePages()
				.OfType<Post>().OrderByDescending(post => post.Date)
				.AsPageable(true, p ?? 1, CurrentItem.PageSize);
			return View(new BlogViewModel(CurrentItem, recentPosts, null));
		}

        public ActionResult RSSFeed()
		{
            IEnumerable<Post> allPosts = Find.EnumerateAccessibleChildren(CurrentItem)
                 .NavigablePages()
                 .OfType<Post>().OrderByDescending(post => post.Date);
			return View(new BlogViewModel(CurrentItem, null, allPosts));
		}
    }
}