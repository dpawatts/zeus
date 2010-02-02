using Zeus.ContentProperties;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Order Item")]
	[RestrictParents(typeof(Order))]
	public class OrderItem : BaseContentItem
	{
		public ContentItem Product
		{
			get { return GetDetail<ContentItem>("Product", null); }
			set { SetDetail("Product", value); }
		}

		public string ProductTitle
		{
			get { return GetDetail("ProductTitle", string.Empty); }
			set { SetDetail("ProductTitle", value); }
		}

		public int Quantity
		{
			get { return GetDetail("Quantity", 0); }
			set { SetDetail("Quantity", value); }
		}

		public decimal Price
		{
			get { return GetDetail("Price", 0m); }
			set { SetDetail("Price", value); }
		}

		public decimal LineTotal
		{
			get { return Price * Quantity; }
		}

		public PropertyCollection Variations
		{
			get { return GetDetailCollection("Variations", true); }
		}
	}
}