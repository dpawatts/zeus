using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes
{
	[ContentType("Shopping Cart Item")]
	[RestrictParents(typeof(ShoppingCart))]
	public class ShoppingCartItem : BaseContentItem
	{
		
	}
}