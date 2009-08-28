using System.Collections.Generic;
using System.Linq;
using Isis.Web;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
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
			Address billingAddress = shoppingBasket.BillingAddress;
			if (billingAddress == null)
				shoppingBasket.BillingAddress = billingAddress = new Address();
			billingAddress.Title = checkoutDetails.BillingTitle;
			billingAddress.FirstName = checkoutDetails.BillingFirstName;
			billingAddress.Surname = checkoutDetails.BillingSurname;
			billingAddress.AddressLine1 = checkoutDetails.BillingAddressLine1;
			billingAddress.AddressLine2 = checkoutDetails.BillingAddressLine2;
			billingAddress.TownCity = checkoutDetails.BillingTownCity;
			billingAddress.Postcode = checkoutDetails.BillingPostcode;
			if (!checkoutDetails.IsDeliveryAddressSameAsBillingAddress)
			{
				Address shippingAddress = shoppingBasket.ShippingAddress;
				if (shippingAddress == null)
					shoppingBasket.ShippingAddress = shippingAddress = new Address();
				shippingAddress.Title = checkoutDetails.ShippingTitle;
				shippingAddress.FirstName = checkoutDetails.ShippingFirstName;
				shippingAddress.Surname = checkoutDetails.ShippingSurname;
				shippingAddress.AddressLine1 = checkoutDetails.ShippingAddressLine1;
				shippingAddress.AddressLine2 = checkoutDetails.ShippingAddressLine2;
				shippingAddress.TownCity = checkoutDetails.ShippingTownCity;
				shippingAddress.Postcode = checkoutDetails.ShippingPostcode;
			}
			else
			{
				if (shoppingBasket.ShippingAddress != null)
					Engine.Persister.Delete(shoppingBasket.ShippingAddress);
			}
			PaymentCard paymentCard = shoppingBasket.PaymentCard;
			if (paymentCard == null)
				shoppingBasket.PaymentCard = paymentCard = new PaymentCard();
			paymentCard.CardType = checkoutDetails.CardType;
			paymentCard.NameOnCard = checkoutDetails.CardHolderName;
			paymentCard.MaskedCardNumber = _shoppingBasketService.GetMaskedCardNumber(checkoutDetails.CardNumber);
			paymentCard.ExpiryMonth = checkoutDetails.CardExpiryMonth.Value;
			paymentCard.ExpiryYear = checkoutDetails.CardExpiryYear.Value;
			paymentCard.StartMonth = checkoutDetails.CardStartMonth;
			paymentCard.StartYear = checkoutDetails.CardStartYear;
			paymentCard.IssueNumber = checkoutDetails.CardIssueNumber;
			shoppingBasket.EmailAddress = checkoutDetails.EmailAddress;
			shoppingBasket.TelephoneNumber = checkoutDetails.TelephoneNumber;

			_shoppingBasketService.SaveBasket(CurrentShop);

			// Handle card number and CVC code.
			TempData["CardNumber"] = checkoutDetails.CardNumber;
			TempData["CardVerificationCode"] = checkoutDetails.CardVerificationCode;

			return Redirect(new Url(CurrentItem.Url).AppendSegment("summary"));
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

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Summary()
		{
			return View(new CheckoutPageSummaryViewModel(CurrentItem, GetShoppingBasket()));
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Summary(object value)
		{
			return View(new CheckoutPageSummaryViewModel(CurrentItem, GetShoppingBasket()));
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