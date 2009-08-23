using System;
using Isis.Collections.Generic;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Mvc.ViewModels;
using Zeus.Templates.Mvc.Controllers;
using System.Web.Mvc;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
	[Controls(typeof(Subcategory), AreaName = ECommerceWebPackage.AREA_NAME)]
	public class SubcategoryController : ZeusController<Subcategory>
	{
		[ActionName("NotUsed")]
		public override ActionResult Index()
		{
			throw new NotSupportedException();
		}

		public ActionResult Index(int? page, bool? viewAll)
		{
			return View(new SubcategoryViewModel(CurrentItem,
				(Category) CurrentItem.Parent,
				CurrentItem.Parent.GetChildren<Subcategory>(),
				CurrentItem.GetChildren<Product>().AsPageable(!(viewAll ?? false), page ?? 1, 9)));
		}
	}
}