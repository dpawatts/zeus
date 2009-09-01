using System.Collections.Generic;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Variation Set")]
	[RestrictParents(typeof(VariationSetContainer))]
	public class VariationSet : BaseContentItem
	{
		public override string IconUrl
		{
			get { return GetIconUrl(typeof(VariationSet), "Zeus.AddIns.ECommerce.Icons.arrow_join.png"); }
		}

		public IEnumerable<Variation> Variations
		{
			get { return GetChildren<Variation>(); }
		}
	}
}