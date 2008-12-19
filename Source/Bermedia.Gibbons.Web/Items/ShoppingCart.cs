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

		public decimal TotalPrice
		{
			get
			{
				decimal result = 0;
				foreach (ShoppingCartItem shoppingCartItem in this.Children)
					result += shoppingCartItem.Price;
				return result;
			}
		}
	}
}
