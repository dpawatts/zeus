using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "[Internal Use Only]")]
	[RestrictParents(typeof(ShoppingCartContainer))]
	public class ShoppingCart : BaseContentItem
	{
		protected override string IconName
		{
			get { return "page"; }
		}

		public BaseDeliveryType DeliveryType
		{
			get { return GetDetail<BaseDeliveryType>("DeliveryType", null); }
			set { SetDetail<BaseDeliveryType>("DeliveryType", value); }
		}

		public Address ShippingAddress
		{
			get { return GetDetail<Address>("ShippingAddress", null); }
			set { SetDetail<Address>("ShippingAddress", value); }
		}

		public Address BillingAddress
		{
			get { return GetDetail<Address>("BillingAddress", null); }
			set { SetDetail<Address>("BillingAddress", value); }
		}

		public decimal ItemTotalPrice
		{
			get
			{
				decimal result = 0;
				foreach (ShoppingCartItem shoppingCartItem in this.Children)
					result += shoppingCartItem.Price;
				return result;
			}
		}

		public decimal TotalPrice
		{
			get
			{
				decimal result = this.ItemTotalPrice;
				if (this.DeliveryType != null)
					result += this.DeliveryType.GetPrice(this);
				return result;
			}
		}
	}
}
