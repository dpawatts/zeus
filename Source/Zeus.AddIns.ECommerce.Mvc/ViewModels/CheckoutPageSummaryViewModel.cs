using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Services;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class CheckoutPageSummaryViewModel : ViewModel<CheckoutPage>
	{
		public CheckoutPageSummaryViewModel(CheckoutPage currentItem, IShoppingBasket shoppingBasket,
			ShoppingBasketPage shoppingBasketPage)
			: base(currentItem)
		{
			ShoppingBasket = shoppingBasket;
			ShoppingBasketPage = shoppingBasketPage;
		}

		public IShoppingBasket ShoppingBasket { get; set; }
		public ShoppingBasketPage ShoppingBasketPage { get; set; }
	}
}