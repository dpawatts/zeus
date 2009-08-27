using System;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.ContentTypes;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType(Name = "BaseShop")]
	public class Shop : BasePage, ISelfPopulator
	{
		private const string SHOPPING_BASKETS_NAME = "shopping-baskets";
		private const string DELIVERY_METHODS_NAME = "delivery-methods";
		private const string CHECKOUT_NAME = "checkout";

		public override string IconUrl
		{
			get { return GetIconUrl(typeof(Shop), "Zeus.AddIns.ECommerce.Icons.money.png"); }
		}

		public ShoppingBasketContainer ShoppingBaskets
		{
			get { return GetChild(SHOPPING_BASKETS_NAME) as ShoppingBasketContainer; }
		}

		public DeliveryMethodContainer DeliveryMethods
		{
			get { return GetChild(DELIVERY_METHODS_NAME) as DeliveryMethodContainer; }
		}

		public CheckoutPage CheckoutPage
		{
			get { return GetChild(CHECKOUT_NAME) as CheckoutPage; }
		}

		void ISelfPopulator.Populate()
		{
			ShoppingBasketContainer shoppingBaskets = new ShoppingBasketContainer
			{
				Name = SHOPPING_BASKETS_NAME,
				Title = "Shopping Baskets"
			};
			shoppingBaskets.AddTo(this);

			DeliveryMethodContainer deliveryMethods = new DeliveryMethodContainer
			{
				Name = DELIVERY_METHODS_NAME,
				Title = "Delivery Methods"
			};
			deliveryMethods.AddTo(this);

			CheckoutPage checkoutPage = new CheckoutPage
			{
				Name = CHECKOUT_NAME,
				Title = "Checkout"
			};
			checkoutPage.AddTo(this);
		}
	}
}