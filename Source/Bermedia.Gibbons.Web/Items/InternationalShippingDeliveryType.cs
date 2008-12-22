using System;
using System.Linq;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("International Shipping Delivery Type")]
	[RestrictParents(typeof(InternationalShippingRateContainer))]
	public class InternationalShippingDeliveryType : BaseDeliveryType
	{
		[TextBoxEditor("TitleWithLineBreak", 15, Required = true)]
		public string TitleWithLineBreak
		{
			get { return GetDetail<string>("TitleWithLineBreak", string.Empty); }
			set { SetDetail<string>("TitleWithLineBreak", value); }
		}

		public override decimal GetPrice(ShoppingCart shoppingCart)
		{
			// Look up InternationalShippingRate for this delivery type and the price of the shopping cart.
			InternationalShippingRate rate = Zeus.Context.Current.Finder.OfType<InternationalShippingRate>()
				.SingleOrDefault(r => r.DeliveryType == this && r.PriceRange.Matches(shoppingCart.ItemTotalPrice));
			if (rate == null)
				throw new ZeusException("Could not find shipping rate matching the total price");
			return rate.Price;
		}
	}
}
