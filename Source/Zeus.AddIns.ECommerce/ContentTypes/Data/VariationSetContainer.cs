using System.Collections.Generic;
using Ext.Net;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Variation Set Container")]
	[RestrictParents(typeof(Shop))]
	public class VariationSetContainer : BaseContentItem
	{
		protected override Icon Icon
		{
			get { return Icon.ArrowDivide; }
		}

		public IEnumerable<VariationSet> Sets
		{
			get { return GetChildren<VariationSet>(); }
		}
	}
}