using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(Category), AreaName = BlogsWebPackage.AREA_NAME)]
	public class CategoryController : ZeusController<Category>
	{
		public override ActionResult Index()
		{
			Blog blog = (Blog) CurrentItem.Parent.Parent;
			IEnumerable<Post> posts = Find.EnumerateAccessibleChildren(blog)
				//.Where(p => p.IsPublished())
				.OfType<Post>()
				.Where(p => p.Categories.Cast<Category>().Contains(CurrentItem))
				.OrderByDescending(p => p.Date);
			return View(new CategoryViewModel(CurrentItem, posts));
		}
	}
}