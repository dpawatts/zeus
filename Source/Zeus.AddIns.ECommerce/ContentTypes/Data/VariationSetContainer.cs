using System.Collections.Generic;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Variation Set Container")]
	[RestrictParents(typeof(Shop))]
	public class VariationSetContainer : BaseContentItem
	{
		public override string IconUrl
		{
			get { return GetIconUrl(typeof(VariationSetContainer), "Zeus.AddIns.ECommerce.Icons.arrow_divide.png"); }
		}

		public IEnumerable<VariationSet> Sets
		{
			get { return GetChildren<VariationSet>(); }
		}
	}
}