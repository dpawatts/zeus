using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Shopping Basket Item")]
	[RestrictParents(typeof(ShoppingBasket))]
	public class ShoppingBasketItem : BaseContentItem
	{
		public Product Product
		{
			get { return GetDetail<Product>("Product", null); }
			set { SetDetail("Product", value); }
		}

		public int Quantity
		{
			get { return GetDetail("Quantity", 0); }
			set { SetDetail("Quantity", value); }
		}
	}
}