using System;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.Linq;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(Blog), AreaName = BlogsWebPackage.AREA_NAME)]
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
				.Navigable()
				.OfType<Post>().OrderByDescending(post => post.Date)
				.AsPageable(true, p ?? 1, CurrentItem.PageSize);
			return View(new BlogViewModel(CurrentItem, recentPosts));
		}
	}
}