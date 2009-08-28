using System.Collections.Generic;
using System.Linq;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Mvc.ViewModels;
using Zeus.AddIns.ECommerce.Services;
using Zeus.Persistence;
using Zeus.Templates.Mvc.Controllers;
using System.Web.Mvc;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
	[Controls(typeof(ShoppingBasketPage), AreaName = ECommerceWebPackage.AREA_NAME)]
	public class ShoppingBasketPageController : ZeusController<ShoppingBasketPage>
	{
		private readonly IShoppingBasketService _shoppingBasketService;
		private readonly IFinder<DeliveryMethod> _deliveryMethodFinder;

		public ShoppingBasketPageController(IShoppingBasketService shoppingBasketService, IFinder<DeliveryMethod> deliveryMethodFinder)
		{
			_shoppingBasketService = shoppingBasketService;
			_deliveryMethodFinder = deliveryMethodFinder;
		}

		protected Shop CurrentShop
		{
			get { return (Shop) CurrentItem.Parent; }
		}

		public override ActionResult Index()
		{
			IShoppingBasket shoppingBasket = GetShoppingBasket();
			IEnumerable<SelectListItem> deliveryMethods = CurrentShop.DeliveryMethods.GetChildren<DeliveryMethod>()
				.Select(dm => new SelectListItem
				{
					Text = dm.Title,
					Value = dm.ID.ToString(),
					Selected = (dm == shoppingBasket.DeliveryMethod)
				});
			return View(new ShoppingBasketPageViewModel(CurrentItem, GetShoppingBasket(), deliveryMethods, CurrentShop.CheckoutPage));
		}

		public ActionResult UpdateQuantity(
			[Bind(Prefix = "id")] int productID,
			[Bind(Prefix = "qty")] int quantity)
		{
			Product product = Engine.Persister.Get<Product>(productID);
			_shoppingBasketService.UpdateQuantity(CurrentShop, product, quantity);
			
			return Redirect(CurrentItem.Url);
		}

		public ActionResult UpdateDeliveryMethod(int deliveryMethodID)
		{
			IShoppingBasket shoppingBasket = GetShoppingBasket();
			shoppingBasket.DeliveryMethod = _deliveryMethodFinder.Items().Single(dm => dm.ID == deliveryMethodID);
			_shoppingBasketService.SaveBasket(CurrentShop);

			return Redirect(CurrentItem.Url);
		}

		private IShoppingBasket GetShoppingBasket()
		{
			return _shoppingBasketService.GetBasket(CurrentShop);
		}
	}
}