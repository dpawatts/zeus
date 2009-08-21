using System;
using MvcContrib.Pagination;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Mvc.ViewModels;
using Zeus.Templates.Mvc.Controllers;
using System.Web.Mvc;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
	[Controls(typeof(Category), AreaName = ECommerceWebPackage.AREA_NAME)]
	public class CategoryController : ZeusController<Category>
	{
		public ActionResult Index(int? p)
		{
			return View(new CategoryViewModel(CurrentItem,
				CurrentItem.GetChildren<Subcategory>(),
				CurrentItem.GetChildren<Product>()));
		}
	}
}