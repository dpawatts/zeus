using System;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.AddIns.Blogs.Services;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.Linq;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(Category), AreaName = BlogsAreaRegistration.AREA_NAME)]
	public class CategoryController : ZeusController<Category>
	{
		private readonly IBlogService _blogService;

		public CategoryController(IBlogService blogService)
		{
			_blogService = blogService;
		}

		[NonAction]
		public override ActionResult Index()
		{
			throw new InvalidOperationException();
		}

		public ActionResult Index(int? p)
		{
			Blog blog = (Blog) CurrentItem.Parent.Parent;
			IPageable<Post> posts = _blogService.GetPostsInCategory(blog, CurrentItem)
				.AsPageable(true, p ?? 1, blog.PageSize);
			return View(new CategoryViewModel(CurrentItem, posts));
		}
	}
}