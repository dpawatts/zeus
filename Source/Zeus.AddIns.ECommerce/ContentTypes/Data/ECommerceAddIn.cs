using Ext.Net;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("ECommerce AddIn")]
	[RestrictParents(typeof(AddInContainer))]
	public class ECommerceAddIn : DataContentItem, IECommerceConfiguration, ISelfPopulator
	{
		private const string ORDERS_NAME = "orders";

		public ECommerceAddIn()
		{
			Title = "ECommerce";
			Name = "ecommerce";
		}

		protected override Icon Icon
		{
			get { return Icon.Plugin; }
		}

		[ContentProperty("Confirmation Email From", 220)]
		public string ConfirmationEmailFrom
		{
			get { return GetDetail("ConfirmationEmailFrom", string.Empty); }
			set { SetDetail("ConfirmationEmailFrom", value); }
		}

		[ContentProperty("Confirmation Email Text", 221), TextBoxEditor(TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine)]
		public string ConfirmationEmailText
		{
			get { return GetDetail("ConfirmationEmailText", string.Empty); }
			set { SetDetail("ConfirmationEmailText", value); }
		}

		[ContentProperty("Vendor Email", 222, Description = "This is the email address which will receive the vendor's copy of the order confirmation email.")]
		public string VendorEmail
		{
			get { return GetDetail("VendorEmail", string.Empty); }
			set { SetDetail("VendorEmail", value); }
		}

		public OrderContainer Orders
		{
			get { return GetChild(ORDERS_NAME) as OrderContainer; }
		}

		void ISelfPopulator.Populate()
		{
			OrderContainer orders = new OrderContainer
			{
				Name = ORDERS_NAME,
				Title = "Orders"
			};
			orders.AddTo(this);
		}
	}
}