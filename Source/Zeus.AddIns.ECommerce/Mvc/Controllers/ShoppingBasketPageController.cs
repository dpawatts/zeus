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
using Zeus.Web.Mvc;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
	[Controls(typeof(ShoppingBasketPage), AreaName = ECommerceAreaRegistration.AREA_NAME)]
	public class ShoppingBasketPageController : ZeusController<ShoppingBasketPage>
	{
		private readonly IShoppingBasketService _shoppingBasketService;
		private readonly IFinder _finder;

		public ShoppingBasketPageController(IShoppingBasketService shoppingBasketService, IFinder finder)
		{
			_shoppingBasketService = shoppingBasketService;
			_finder = finder;
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
			[Bind(Prefix = "varid")] int variationPermutationID,
			[Bind(Prefix = "qty")] int quantity)
		{
			Product product = Engine.Persister.Get<Product>(productID);
			VariationPermutation variationPermutation = Engine.Persister.Get<VariationPermutation>(variationPermutationID);
			_shoppingBasketService.UpdateQuantity(CurrentShop, product, variationPermutation, quantity);
			
			return Redirect(CurrentItem.Url);
		}

		[ActionName("Checkout")]
		[AcceptVerbs(HttpVerbs.Post)]
		[AcceptImageSubmit(Name = "updateDeliveryMethod")]
		public ActionResult UpdateDeliveryMethod(int deliveryMethodID)
		{
			UpdateDeliveryMethodInternal(deliveryMethodID);
			return Redirect(CurrentItem.Url);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[AcceptImageSubmit(Name = "checkout")]
		public ActionResult Checkout(int deliveryMethodID)
		{
			UpdateDeliveryMethodInternal(deliveryMethodID);

			// Validate that there are items in the shopping basket.
			if (!GetShoppingBasket().Items.Any())
				return RedirectToParentPage();

			return Redirect(CurrentShop.CheckoutPage.Url);
		}

		private IShoppingBasket GetShoppingBasket()
		{
			return _shoppingBasketService.GetBasket(CurrentShop);
		}

		private void UpdateDeliveryMethodInternal(int deliveryMethodID)
		{
			IShoppingBasket shoppingBasket = GetShoppingBasket();
			shoppingBasket.DeliveryMethod = _finder.QueryItems<DeliveryMethod>().Single(dm => dm.ID == deliveryMethodID);
			_shoppingBasketService.SaveBasket(CurrentShop);
		}
	}
}