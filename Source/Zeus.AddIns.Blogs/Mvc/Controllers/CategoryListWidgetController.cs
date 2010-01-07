using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.AddIns.Blogs.Services;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(CategoryListWidget), AreaName = BlogsWebPackage.AREA_NAME)]
	public class CategoryListWidgetController : WidgetController<CategoryListWidget>
	{
		private readonly IBlogService _blogService;

		public CategoryListWidgetController(IBlogService blogService)
		{
			_blogService = blogService;
		}

		public override ActionResult Index()
		{
			var categoryEntries = CurrentItem.Blog.Categories.GetChildren<Category>()
				.Select(c =>
					new CategoryListCategoryEntry
						{
							Category = c,
							Count = _blogService.GetPostsInCategory(CurrentItem.Blog, c).Count()
						}
				);
			return PartialView(new CategoryListWidgetViewModel(CurrentItem, categoryEntries));
		}
	}
}