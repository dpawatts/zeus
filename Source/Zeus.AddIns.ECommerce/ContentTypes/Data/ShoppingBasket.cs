using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.AddIns.ECommerce.Services;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Shopping Basket")]
	[RestrictParents(typeof(ShoppingBasketContainer))]
	public class ShoppingBasket : BaseContentItem, IShoppingBasket
	{
		public IEnumerable<IShoppingBasketItem> Items
		{
			get { return GetChildren<ShoppingBasketItem>().Cast<IShoppingBasketItem>(); }
		}

		public decimal TotalItemPrice
		{
			get { return Items.Sum(i => i.Product.CurrentPrice * i.Quantity); }
		}
	}
}