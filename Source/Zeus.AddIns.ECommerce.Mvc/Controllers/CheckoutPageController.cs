using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Mvc.ViewModels;
using Zeus.AddIns.ECommerce.Services;
using Zeus.Templates.Mvc.Controllers;
using System.Web.Mvc;
using Zeus.Web;
using Zeus.Web.Mvc;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
	[Controls(typeof(CheckoutPage), AreaName = ECommerceWebPackage.AREA_NAME)]
	[RequireSsl]
	public class CheckoutPageController : ZeusController<CheckoutPage>
	{
		private readonly IShoppingBasketService _shoppingBasketService;

		public CheckoutPageController(IShoppingBasketService shoppingBasketService)
		{
			_shoppingBasketService = shoppingBasketService;
		}

		protected Shop CurrentShop
		{
			get { return (Shop) CurrentItem.Parent; }
		}

		public override ActionResult Index()
		{
			return View(new CheckoutPageViewModel(CurrentItem));
		}

		public ActionResult Summary()
		{
			return View(new CheckoutPageSummaryViewModel(CurrentItem));
		}

		public ActionResult Receipt()
		{
			return View(new CheckoutPageReceiptViewModel(CurrentItem));
		}

		private IShoppingBasket GetShoppingBasket()
		{
			return _shoppingBasketService.GetBasket(CurrentShop);
		}
	}
}