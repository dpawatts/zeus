using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(CategoryContainer), AreaName = BlogsAreaRegistration.AREA_NAME)]
	public class CategoryContainerController : ZeusController<CategoryContainer>
	{
		public override ActionResult Index()
		{
			return View(new CategoryContainerViewModel(CurrentItem, CurrentItem.GetChildren<Category>()));
		}
	}
}