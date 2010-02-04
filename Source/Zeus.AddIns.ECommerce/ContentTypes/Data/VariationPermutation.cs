using System.Linq;
using Ext.Net;
using Zeus.BaseLibrary.ExtensionMethods.Linq;
using Zeus.ContentProperties;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType]
	[RestrictParents(typeof(ShoppingBasketItem), typeof(VariationConfiguration), typeof(OrderItem))]
	public class VariationPermutation : BaseContentItem, ILink
	{
		protected override Icon Icon
		{
			get { return Icon.ArrowMerge; }
		}

		string ILink.Contents
		{
			get { return Variations.Cast<Variation>().Join(v => v.Title, ", "); }
		}

		public PropertyCollection Variations
		{
			get { return GetDetailCollection("Variations", true); }
		}
	}
}