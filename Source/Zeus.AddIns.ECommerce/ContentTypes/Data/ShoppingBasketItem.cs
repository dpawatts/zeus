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
	[ContentType("Shopping Basket Item")]
	[RestrictParents(typeof(ShoppingBasket))]
	public class ShoppingBasketItem : BaseContentItem, IShoppingBasketItem
	{
		public Product Product
		{
			get { return GetDetail<Product>("Product", null); }
			set { SetDetail("Product", value); }
		}

		public PropertyCollection VariationChoices
		{
			get { return GetDetailCollection("VariationChoices", true); }
		}

		public int Quantity
		{
			get { return GetDetail("Quantity", 0); }
			set { SetDetail("Quantity", value); }
		}

		public decimal LineTotal
		{
			get { return Product.CurrentPrice * Quantity; }
		}

		public IEnumerable<Variation> Variations
		{
			get { return VariationChoices.Cast<Variation>(); }
		}
	}
}