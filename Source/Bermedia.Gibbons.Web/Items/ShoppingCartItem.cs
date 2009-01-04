using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "[Internal Use Only]")]
	[RestrictParents(typeof(ShoppingCart))]
	public class ShoppingCartItem : BaseShoppingCartItem
	{
		public StandardProduct Product
		{
			get { return GetDetail<StandardProduct>("StandardProduct", null); }
			set { SetDetail<StandardProduct>("StandardProduct", value); }
		}

		public override string ProductTitle
		{
			get { return this.Product.DisplayTitle; }
		}

		public ProductSizeLink Size
		{
			get { return GetDetail<ProductSizeLink>("Size", null); }
			set { SetDetail<ProductSizeLink>("Size", value); }
		}

		public override string ProductSizeTitle
		{
			get { return (this.Size != null) ? this.Size.Title : string.Empty; }
		}

		public ProductColour Colour
		{
			get { return GetDetail<ProductColour>("Colour", null); }
			set { SetDetail<ProductColour>("Colour", value); }
		}

		public override string ProductColourTitle
		{
			get { return (this.Colour != null) ? this.Colour.Title : string.Empty; }
		}

		public override decimal PricePerUnit
		{
			get
			{
				if (this.Size != null && this.Size.CurrentPrice != null)
					return this.Size.CurrentPrice.Value;
				else
					return this.Product.CurrentPrice;
			}
			set { throw new NotImplementedException(); }
		}
	}
}
