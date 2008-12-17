using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("International Shipping Rate Container", Description = "Container for international shipping rates")]
	[RestrictParents(typeof(RootItem))]
	public class InternationalShippingRateContainer : BaseContentItem
	{
		public InternationalShippingRateContainer()
		{
			this.Name = "InternationalShippingRates";
			this.Title = "International Shipping Rates";
		}

		protected override string IconName
		{
			get { return "package_go"; }
		}

		public override string TemplateUrl
		{
			get { return "~/Admin/View.aspx?selected=" + this.Path; }
		}
	}
}
