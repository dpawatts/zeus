using System;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Templates.ContentTypes;
using Zeus.Web.UI;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType(Name = "BaseShop")]
	[TabPanel("ECommerce", "E-Commerce", 100)]
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

		[StringCollectionEditor("Titles", 200, ContainerName = "ECommerce")]
		public PropertyCollection Titles
		{
			get { return GetDetailCollection("Titles", true); }
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