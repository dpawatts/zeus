using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("International Shipping Rate")]
	[RestrictParents(typeof(InternationalShippingRateContainer))]
	public class InternationalShippingRate : BaseContentItem
	{
		[LinkedItemDropDownListEditor("Price Range", 10, TypeFilter = typeof(InternationalShippingPriceRange), Required = true)]
		public virtual InternationalShippingPriceRange PriceRange
		{
			get { return GetDetail<InternationalShippingPriceRange>("PriceRange", null); }
			set { SetDetail<InternationalShippingPriceRange>("PriceRange", value); }
		}

		[LinkedItemDropDownListEditor("Delivery Type", 20, TypeFilter = typeof(InternationalShippingDeliveryType), Required = true)]
		public virtual InternationalShippingDeliveryType DeliveryType
		{
			get { return GetDetail<InternationalShippingDeliveryType>("DeliveryType", null); }
			set { SetDetail<InternationalShippingDeliveryType>("DeliveryType", value); }
		}

		[TextBoxEditor("Price", 30)]
		public decimal Price
		{
			get { return GetDetail<decimal>("Price", 0); }
			set { SetDetail<decimal>("Price", value); }
		}

		protected override string IconName
		{
			get { return "package_go"; }
		}
	}
}
