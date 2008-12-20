using System;
using Zeus;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Delivery Type")]
	public class DeliveryType : BaseDeliveryType
	{
		[TextBoxEditor("Price", 30, Required = true)]
		public decimal Price
		{
			get { return GetDetail<decimal>("Price", 0); }
			set { SetDetail<decimal>("Price", value); }
		}

		public override decimal GetPrice(ShoppingCart shoppingCart)
		{
			return this.Price;
		}
	}
}
