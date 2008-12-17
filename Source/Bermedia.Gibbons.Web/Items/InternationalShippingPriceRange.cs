using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("International Shipping Price Range")]
	[RestrictParents(typeof(InternationalShippingRateContainer))]
	public class InternationalShippingPriceRange : BaseContentItem
	{
		[LiteralDisplayer(Title = "Name")]
		[TextBoxEditor("Name", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[TextBoxEditor("Minimum Price", 10, Required = true)]
		public decimal MinimumPrice
		{
			get { return GetDetail<decimal>("MinimumPrice", 0); }
			set { SetDetail<decimal>("MinimumPrice", value); }
		}

		[TextBoxEditor("Maximum Price", 10)]
		public decimal? MaximumPrice
		{
			get { return GetDetail<decimal?>("MaximumPrice", null); }
			set { SetDetail<decimal?>("MaximumPrice", value); }
		}

		protected override string IconName
		{
			get { return "money"; }
		}
	}
}
