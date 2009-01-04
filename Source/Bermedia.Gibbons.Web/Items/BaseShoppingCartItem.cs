using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	public abstract class BaseShoppingCartItem : BaseContentItem
	{
		public abstract string ProductTitle
		{
			get;
		}

		public abstract string ProductSizeTitle
		{
			get;
		}

		public abstract string ProductColourTitle
		{
			get;
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

		public abstract decimal PricePerUnit
		{
			get;
			set;
		}

		public decimal Price
		{
			get { return this.PricePerUnit * this.Quantity; }
		}

		protected override string IconName
		{
			get { return "page"; }
		}
	}
}
