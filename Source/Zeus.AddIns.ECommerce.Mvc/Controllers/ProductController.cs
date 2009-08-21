using System.Web.Mvc;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Mvc.ViewModels;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
	[Controls(typeof(Product), AreaName = ECommerceWebPackage.AREA_NAME)]
	public class ProductController : ZeusController<Product>
	{
		public override ActionResult Index()
		{
			return View(new ProductViewModel(CurrentItem,
				(Category) CurrentItem.Parent.Parent,
				CurrentItem.Parent.Parent.GetChildren<Subcategory>()));
		}
	}
}