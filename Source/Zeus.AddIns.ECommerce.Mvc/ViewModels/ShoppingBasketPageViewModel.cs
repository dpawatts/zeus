using System.Collections.Generic;
using System.Web.Mvc;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Services;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class ShoppingBasketPageViewModel : ViewModel<ShoppingBasketPage>
	{
		public ShoppingBasketPageViewModel(ShoppingBasketPage currentItem, IShoppingBasket shoppingBasket,
			IEnumerable<SelectListItem> deliveryMethods)
			: base(currentItem)
		{
			ShoppingBasket = shoppingBasket;
			DeliveryMethods = deliveryMethods;
		}

		public IShoppingBasket ShoppingBasket { get; set; }
		public IEnumerable<SelectListItem> DeliveryMethods { get; set; }
	}
}