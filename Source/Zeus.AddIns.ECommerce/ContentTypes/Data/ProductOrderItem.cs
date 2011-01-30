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

		public int WeakProductLink
		{
            get { return GetDetail("WeakProductLink", 0); }
            set { SetDetail("WeakProductLink", value); }
		}

        public Zeus.AddIns.ECommerce.ContentTypes.Pages.Product Product {
            get { return (Zeus.AddIns.ECommerce.ContentTypes.Pages.Product)Zeus.Context.Persister.Get(WeakProductLink); }
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