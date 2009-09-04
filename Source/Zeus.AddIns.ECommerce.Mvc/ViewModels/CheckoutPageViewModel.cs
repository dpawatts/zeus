using System.Collections.Generic;
using System.Web.Mvc;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class CheckoutPageViewModel : ViewModel<CheckoutPage>
	{
		public CheckoutPageViewModel(CheckoutPage currentItem,
			IEnumerable<SelectListItem> billingAddressTitles,
			IEnumerable<SelectListItem> shippingAddressTitles,
			IEnumerable<SelectListItem> cardTypes,
			ShoppingBasketPage shoppingBasketPage)
			: base(currentItem)
		{
			BillingAddressTitles = billingAddressTitles;
			ShippingAddressTitles = shippingAddressTitles;
			CardTypes = cardTypes;
			ShoppingBasketPage = shoppingBasketPage;
		}

		public IEnumerable<SelectListItem> BillingAddressTitles { get; set; }
		public IEnumerable<SelectListItem> ShippingAddressTitles { get; set; }
		public IEnumerable<SelectListItem> CardTypes { get; set; }
		public ShoppingBasketPage ShoppingBasketPage { get; set; }
	}
}