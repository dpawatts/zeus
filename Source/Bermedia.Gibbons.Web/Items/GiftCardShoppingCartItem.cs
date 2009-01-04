using System;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "[Internal Use Only]")]
	[RestrictParents(typeof(ShoppingCart))]
	public class GiftCardShoppingCartItem : BaseShoppingCartItem
	{
		public string RecipientName
		{
			get { return GetDetail<string>("RecipientName", string.Empty); }
			set { SetDetail<string>("RecipientName", value); }
		}

		public override string ProductTitle
		{
			get { return string.Format("Gift Card for '{0}'", this.RecipientName); }
		}

		public override string ProductSizeTitle
		{
			get { return string.Empty; }
		}

		public override string ProductColourTitle
		{
			get { return string.Empty; }
		}

		public override decimal PricePerUnit
		{
			get { return GetDetail<decimal>("PricePerUnit", 0); }
			set { SetDetail<decimal>("PricePerUnit", value); }
		}

		public Zeus.AddIns.Images.Items.Image Image
		{
			get { return GetDetail<Zeus.AddIns.Images.Items.Image>("Image", null); }
			set { SetDetail<Zeus.AddIns.Images.Items.Image>("Image", value); }
		}
	}
}
