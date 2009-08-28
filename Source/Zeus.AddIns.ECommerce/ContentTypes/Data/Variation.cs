using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType]
	[RestrictParents(typeof(VariationSet))]
	public class Variation : BaseContentItem
	{
		public VariationSet VariationSet
		{
			get { return (VariationSet)Parent; }
		}
	}
}