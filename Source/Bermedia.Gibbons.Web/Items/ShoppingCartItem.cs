﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "[Internal Use Only]")]
	[RestrictParents(typeof(ShoppingCart))]
	public class ShoppingCartItem : BaseContentItem
	{
		public StandardProduct Product
		{
			get { return GetDetail<StandardProduct>("StandardProduct", null); }
			set { SetDetail<StandardProduct>("StandardProduct", value); }
		}

		public ProductSizeLink Size
		{
			get { return GetDetail<ProductSizeLink>("Size", null); }
			set { SetDetail<ProductSizeLink>("Size", value); }
		}

		public ProductColour Colour
		{
			get { return GetDetail<ProductColour>("Colour", null); }
			set { SetDetail<ProductColour>("Colour", value); }
		}

		public int Quantity
		{
			get { return GetDetail<int>("Quantity", 0); }
			set { SetDetail<int>("Quantity", value); }
		}

		public bool IsGift
		{
			get { return GetDetail<bool>("IsGift", false); }
			set { SetDetail<bool>("IsGift", value); }
		}

		public GiftWrapType GiftWrapType
		{
			get { return GetDetail<GiftWrapType>("GiftWrapType", null); }
			set { SetDetail<GiftWrapType>("GiftWrapType", value); }
		}

		public decimal Price
		{
			get
			{
				decimal currentPrice = 0;
				if (this.Size != null && this.Size.CurrentPrice != null)
					currentPrice = this.Size.CurrentPrice.Value;
				else
					currentPrice = this.Product.CurrentPrice;
				return currentPrice * this.Quantity;
			}
		}

		protected override string IconName
		{
			get { return "page"; }
		}
	}
}
