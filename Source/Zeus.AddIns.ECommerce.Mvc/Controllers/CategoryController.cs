using System;
using System.Linq;
using Isis.Collections.Generic;
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
		[NonAction]
		public override ActionResult Index()
		{
			throw new NotSupportedException();
		}

		public ActionResult Index(int? page, bool? viewAll)
		{
			var subcategories = CurrentItem.GetChildren<Subcategory>();
			if (subcategories.Any())
				return View(new CategoryViewModel(CurrentItem,
					subcategories, CurrentItem.GetChildren<Product>()));
			return View("IndexNoSubcategories", new CategoryNoSubcategoriesViewModel(CurrentItem,
				CurrentItem.GetChildren<Product>().AsPageable(!(viewAll ?? false), page ?? 1, 8)));
		}
	}
}