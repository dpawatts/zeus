using System.Collections.Generic;
using System.Linq;
using Isis.ExtensionMethods;
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

			return View("Summary", new CheckoutPageSummaryViewModel(CurrentItem, GetShoppingBasket()));
		}

		private ActionResult GetIndexView()
		{
			IEnumerable<SelectListItem> cardTypes = new [] { new SelectListItem { Text = "Please select...", Value = "" }}
				.Union(_shoppingBasketService.GetSupportedCardTypes().Select(pct => new SelectListItem
				{
					Text = pct.GetDescription(),
					Value = pct.ToString()
				}));
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

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult PlaceOrder(string cardNumber,
			[Bind(Prefix = "cvc")] string cardVerificationCode)
		{
			try
			{
				Order order = _shoppingBasketService.PlaceOrder(CurrentShop, cardNumber, cardVerificationCode);
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