using System.Collections.Generic;
using System.Linq;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Mvc.ViewModels;
using Zeus.AddIns.ECommerce.Services;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.Templates.Mvc.Controllers;
using System.Web.Mvc;
using Zeus.Web;
using Zeus.Web.Mvc;
using Zeus.Web.Mvc.ActionFilters;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
	[Controls(typeof(CheckoutPage), AreaName = ECommerceAreaRegistration.AREA_NAME)]
	[RequireSsl]
	public class CheckoutPageController : ZeusController<CheckoutPage>
	{
		private readonly IShoppingBasketService _shoppingBasketService;
		private readonly IOrderService _orderService;

		public CheckoutPageController(IShoppingBasketService shoppingBasketService, IOrderService orderService)
		{
			_shoppingBasketService = shoppingBasketService;
			_orderService = orderService;
		}

		protected Shop CurrentShop
		{
			get { return (Shop) CurrentItem.Parent; }
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public override ActionResult Index()
		{
			// If we already have shopping basket data entered, add it to ModelState now.
			IShoppingBasket shoppingBasket = GetShoppingBasket();
			if (shoppingBasket.BillingAddress != null)
			{
				ModelState.Add("BillingTitle", shoppingBasket.BillingAddress.PersonTitle);
				ModelState.Add("BillingFirstName", shoppingBasket.BillingAddress.FirstName);
				ModelState.Add("BillingSurname", shoppingBasket.BillingAddress.Surname);
				ModelState.Add("BillingAddressLine1", shoppingBasket.BillingAddress.AddressLine1);
				ModelState.Add("BillingAddressLine2", shoppingBasket.BillingAddress.AddressLine2);
				ModelState.Add("BillingTownCity", shoppingBasket.BillingAddress.TownCity);
				ModelState.Add("BillingPostcode", shoppingBasket.BillingAddress.Postcode);
			}
			if (shoppingBasket.ShippingAddress != null)
			{
				ModelState.Add("ShippingTitle", shoppingBasket.ShippingAddress.PersonTitle);
				ModelState.Add("ShippingFirstName", shoppingBasket.ShippingAddress.FirstName);
				ModelState.Add("ShippingSurname", shoppingBasket.ShippingAddress.Surname);
				ModelState.Add("ShippingAddressLine1", shoppingBasket.ShippingAddress.AddressLine1);
				ModelState.Add("ShippingAddressLine2", shoppingBasket.ShippingAddress.AddressLine2);
				ModelState.Add("ShippingTownCity", shoppingBasket.ShippingAddress.TownCity);
				ModelState.Add("ShippingPostcode", shoppingBasket.ShippingAddress.Postcode);
			}
			if (shoppingBasket.PaymentCard != null)
			{
				ModelState.Add("CardType", shoppingBasket.PaymentCard.CardType);
				ModelState.Add("CardHolderName", shoppingBasket.PaymentCard.NameOnCard);
				ModelState.Add("CardExpiryMonth", shoppingBasket.PaymentCard.ExpiryMonth);
				ModelState.Add("CardExpiryYear", shoppingBasket.PaymentCard.ExpiryYear);
				ModelState.Add("CardStartMonth", shoppingBasket.PaymentCard.StartMonth);
				ModelState.Add("CardStartYear", shoppingBasket.PaymentCard.StartYear);
				ModelState.Add("CardIssueNumber", shoppingBasket.PaymentCard.IssueNumber);
			}
			ModelState.Add("EmailAddress", shoppingBasket.EmailAddress);
			ModelState.Add("ConfirmEmailAddress", shoppingBasket.EmailAddress);
			ModelState.Add("TelephoneNumber", shoppingBasket.TelephoneNumber);
			ModelState.Add("MobileTelephoneNumber", shoppingBasket.MobileTelephoneNumber);

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
			billingAddress.PersonTitle = checkoutDetails.BillingTitle;
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
				shippingAddress.PersonTitle = checkoutDetails.ShippingTitle;
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
			shoppingBasket.MobileTelephoneNumber = checkoutDetails.MobileTelephoneNumber;

			_shoppingBasketService.SaveBasket(CurrentShop);

			// Handle card number and CVC code.
			TempData["CardNumber"] = checkoutDetails.CardNumber;
			TempData["CardVerificationCode"] = checkoutDetails.CardVerificationCode;

			return View("Summary", new CheckoutPageSummaryViewModel(CurrentItem, GetShoppingBasket(),
				CurrentShop.ShoppingBasketPage));
		}

		private ActionResult GetIndexView()
		{
			IEnumerable<SelectListItem> cardTypes = new [] { new SelectListItem { Text = "Please select...", Value = "" }}
				.Union(_orderService.GetSupportedCardTypes().Select(pct => new SelectListItem
				{
					Text = pct.GetDescription(),
					Value = pct.ToString()
				}));
			IShoppingBasket shoppingBasket = GetShoppingBasket();
			return View("Index", new CheckoutPageViewModel(CurrentItem,
				GetTitles((shoppingBasket.BillingAddress != null) ? shoppingBasket.BillingAddress.Title : null),
				GetTitles((shoppingBasket.ShippingAddress != null) ? shoppingBasket.ShippingAddress.Title : null),
				cardTypes, CurrentShop.ShoppingBasketPage));
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

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult PlaceOrder(string cardNumber,
			[Bind(Prefix = "cvc")] string cardVerificationCode)
		{
			try
			{
				Order order = _orderService.PlaceOrder(CurrentShop, cardNumber, cardVerificationCode);
				return View("Receipt", new CheckoutPageReceiptViewModel(CurrentItem, order.ID.ToString(), CurrentShop.ContactPage));
			}
			catch (ZeusECommerceException ex)
			{
				return View("Failed", new CheckoutPageFailedViewModel(CurrentItem, ex.Message, CurrentShop.ContactPage));
			}
		}

		private IShoppingBasket GetShoppingBasket()
		{
			return _shoppingBasketService.GetBasket(CurrentShop);
		}
	}
}