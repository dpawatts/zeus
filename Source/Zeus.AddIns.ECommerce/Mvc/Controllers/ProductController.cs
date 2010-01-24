using System.Web.Mvc;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Mvc.ViewModels;
using Zeus.AddIns.ECommerce.Services;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
	[Controls(typeof(Product), AreaName = ECommerceAreaRegistration.AREA_NAME)]
	public class ProductController : ProductControllerBase<Product>
	{
		public ProductController(IShoppingBasketService shoppingBasketService)
			: base(shoppingBasketService)
		{
			
		}

		public override ActionResult Index()
		{
			return View(new ProductViewModel(CurrentItem,
				CurrentItem.CurrentCategory,
				CurrentItem.CurrentCategory.GetChildren<Subcategory>()));
		}
	}
}