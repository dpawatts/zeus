using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes
{
	[ContentType("Shopping Cart")]
	[RestrictParents(typeof(Shop))]
	public class ShoppingCart : BaseContentItem
	{
		
	}
}