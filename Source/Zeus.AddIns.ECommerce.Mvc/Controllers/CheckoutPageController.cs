using System.Collections.Generic;
using System.Linq;
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

		[AcceptVerbs(HttpVerbs.Get)]
		public override ActionResult Index()
		{
			return GetIndexView();
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Index(
			[Bind(Prefix="")] CheckoutPageFormModel checkoutDetails)
		{
			if (!ModelState.IsValid)
				return GetIndexView();

			// Map from form model to shopping basket.
			IShoppingBasket shoppingBasket = GetShoppingBasket();
			_shoppingBasketService.SaveBasket(CurrentShop);

			return RedirectToAction("Summary");
		}

		private ActionResult GetIndexView()
		{
			IEnumerable<SelectListItem> cardTypes = new[] { new SelectListItem { Text = "Maestro", Value = "MA" } };
			IShoppingBasket shoppingBasket = GetShoppingBasket();
			return View("Index", new CheckoutPageViewModel(CurrentItem,
				GetTitles((shoppingBasket.BillingAddress != null) ? shoppingBasket.BillingAddress.Title : null),
				GetTitles((shoppingBasket.ShippingAddress != null) ? shoppingBasket.ShippingAddress.Title : null),
				cardTypes));
		}

		private IEnumerable<SelectListItem> GetTitles(string selectedTitle)
		{
			return CurrentShop.Titles.Cast<string>().Select(t => new SelectListItem
			{
				Text = t,
				Value = t,
				Selected = (t == selectedTitle)
			});
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