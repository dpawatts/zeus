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
	[Controls(typeof(BlogYear), AreaName = BlogsAreaRegistration.AREA_NAME)]
	public class BlogYearController : ZeusController<BlogYear>
	{
		[NonAction]
		public override ActionResult Index()
		{
			throw new InvalidOperationException();
		}

		public ActionResult Index(int? p)
		{
			IPageable<Post> posts = Find.EnumerateAccessibleChildren(CurrentItem)
				.NavigablePages()
				.OfType<Post>().OrderByDescending(post => post.Date)
				.AsPageable(true, p ?? 1, CurrentItem.CurrentBlog.PageSize);
			return View(new BlogYearViewModel(CurrentItem, CurrentItem.Year, posts));
		}
	}
}