using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Variation Set")]
	[RestrictParents(typeof(VariationContainer))]
	public class VariationSet : BaseContentItem
	{
		
	}
}