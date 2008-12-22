using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "[Internal Use Only]")]
	[RestrictParents(typeof(Order))]
	public class OrderItem : ShoppingCartItem
	{
		public bool Refunded
		{
			get { return GetDetail<bool>("Refunded", false); }
			set { SetDetail<bool>("Refunded", value); }
		}

		public override decimal PricePerUnit
		{
			get { return GetDetail<decimal>("PricePerUnit", 0); }
			set { SetDetail<decimal>("PricePerUnit", value); }
		}
	}
}
