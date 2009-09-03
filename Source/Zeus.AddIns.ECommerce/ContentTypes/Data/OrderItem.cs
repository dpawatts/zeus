using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.ContentProperties;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.AddIns.ECommerce.Services;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Order Item")]
	[RestrictParents(typeof(Order))]
	public class OrderItem : BaseContentItem
	{
		public Product Product
		{
			get { return GetDetail<Product>("Product", null); }
			set { SetDetail("Product", value); }
		}

		public string ProductTitle
		{
			get { return GetDetail("ProductTitle", string.Empty); }
			set { SetDetail("ProductTitle", value); }
		}

		public int Quantity
		{
			get { return GetDetail("Quantity", 0); }
			set { SetDetail("Quantity", value); }
		}

		public decimal Price
		{
			get { return GetDetail("Price", 0m); }
			set { SetDetail("Price", value); }
		}

		public decimal LineTotal
		{
			get { return Price * Quantity; }
		}

		public PropertyCollection Variations
		{
			get { return GetDetailCollection("Variations", true); }
		}
	}
}