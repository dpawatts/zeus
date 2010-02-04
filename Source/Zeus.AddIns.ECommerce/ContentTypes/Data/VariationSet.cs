using System.Collections.Generic;
using Ext.Net;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Variation Set")]
	[RestrictParents(typeof(VariationSetContainer))]
	public class VariationSet : BaseContentItem
	{
		protected override Icon Icon
		{
			get { return Icon.ArrowJoin; }
		}

		public IEnumerable<Variation> Variations
		{
			get { return GetChildren<Variation>(); }
		}
	}
}