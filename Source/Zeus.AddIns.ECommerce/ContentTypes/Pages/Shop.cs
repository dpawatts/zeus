using System.Linq;
using Ext.Net;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web;
using Zeus.Web.UI;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType(Name = "BaseShop")]
	[TabPanel("ECommerce", "E-Commerce", 100)]
	[RestrictParents(typeof(WebsiteNode), typeof(Page))]
	public class Shop : BasePage, ISelfPopulator
	{
		private const string VARIATION_CONTAINER_NAME = "variations";
		private const string SHOPPING_BASKETS_NAME = "shopping-baskets";
		private const string ORDERS_NAME = "orders";
		private const string DELIVERY_METHODS_NAME = "delivery-methods";
		private const string CHECKOUT_NAME = "checkout";

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Money); }
		}

		public VariationSetContainer VariationsSet
		{
			get { return GetChild(VARIATION_CONTAINER_NAME) as VariationSetContainer; }
		}

		public ShoppingBasketContainer ShoppingBaskets
		{
			get { return GetChild(SHOPPING_BASKETS_NAME) as ShoppingBasketContainer; }
		}

		public OrderContainer Orders
		{
			get { return GetChild(ORDERS_NAME) as OrderContainer; }
		}

		public DeliveryMethodContainer DeliveryMethods
		{
			get { return GetChild(DELIVERY_METHODS_NAME) as DeliveryMethodContainer; }
		}

		public CheckoutPage CheckoutPage
		{
			get { return GetChild(CHECKOUT_NAME) as CheckoutPage; }
		}

		public ShoppingBasketPage ShoppingBasketPage
		{
			get { return GetChildren<ShoppingBasketPage>().FirstOrDefault(); }
		}

		[StringCollectionEditor("Titles", 200, ContainerName = "ECommerce")]
		public PropertyCollection Titles
		{
			get { return GetDetailCollection("Titles", true); }
		}

		[ContentProperty("Contact Page", 210, EditorContainerName = "ECommerce")]
		public ContentItem ContactPage
		{
			get { return GetDetail<ContentItem>("ContactPage", null); }
			set { SetDetail("ContactPage", value); }
		}

		[ContentProperty("Confirmation Email From", 220, EditorContainerName = "ECommerce")]
		public string ConfirmationEmailFrom
		{
			get { return GetDetail("ConfirmationEmailFrom", string.Empty); }
			set { SetDetail("ConfirmationEmailFrom", value); }
		}

		[ContentProperty("Confirmation Email Text", 221, EditorContainerName = "ECommerce"), TextBoxEditor(TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine)]
		public string ConfirmationEmailText
		{
			get { return GetDetail("ConfirmationEmailText", string.Empty); }
			set { SetDetail("ConfirmationEmailText", value); }
		}

		[ContentProperty("Vendor Email", 222, EditorContainerName = "ECommerce", Description = "This is the email address which will receive the vendor's copy of the order confirmation email.")]
		public string VendorEmail
		{
			get { return GetDetail("VendorEmail", string.Empty); }
			set { SetDetail("VendorEmail", value); }
		}

		[ContentProperty("Persistent Shopping Baskets", 222, EditorContainerName = "ECommerce", Description = "Check this box if you want shopping baskets to persist, even after the customer closes their browser.")]
		public bool PersistentShoppingBaskets
		{
			get { return GetDetail("PersistentShoppingBaskets", false); }
			set { SetDetail("PersistentShoppingBaskets", value); }
		}

		void ISelfPopulator.Populate()
		{
			VariationSetContainer variationsSet = new VariationSetContainer
			{
				Name = VARIATION_CONTAINER_NAME,
				Title = "VariationsSet"
			};
			variationsSet.AddTo(this);

			ShoppingBasketContainer shoppingBaskets = new ShoppingBasketContainer
			{
				Name = SHOPPING_BASKETS_NAME,
				Title = "Shopping Baskets"
			};
			shoppingBaskets.AddTo(this);

			OrderContainer orders = new OrderContainer
			{
				Name = ORDERS_NAME,
				Title = "Orders"
			};
			orders.AddTo(this);

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