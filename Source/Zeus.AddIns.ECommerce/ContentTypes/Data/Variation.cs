using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType]
	[RestrictParents(typeof(VariationSet))]
	public class Variation : BaseContentItem
	{
		public override string IconUrl
		{
			get { return GetIconUrl(typeof(Variation), "Zeus.AddIns.ECommerce.Icons.arrow_branch.png"); }
		}

		public VariationSet VariationSet
		{
			get { return (VariationSet)Parent; }
		}
	}
}