using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes
{
	[ContentType("Variation Container")]
	[RestrictParents(typeof(Shop))]
	public class VariationContainer : BaseContentItem
	{
		
	}
}