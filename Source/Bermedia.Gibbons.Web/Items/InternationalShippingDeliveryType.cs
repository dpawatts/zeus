using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("International Shipping Delivery Type")]
	[RestrictParents(typeof(InternationalShippingRateContainer))]
	public class InternationalShippingDeliveryType : BaseContentItem
	{
		[LiteralDisplayer(Title = "Name")]
		[TextBoxEditor("Name", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		protected override string IconName
		{
			get { return "world_go"; }
		}
	}
}
