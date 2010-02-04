using Zeus.ContentProperties;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Product Order Item")]
	public class ProductOrderItem : OrderItem
	{
		public override string DisplayTitle
		{
			get { return ProductTitle; }
		}

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

		public PropertyCollection Variations
		{
			get { return GetDetailCollection("Variations", true); }
		}
	}
}