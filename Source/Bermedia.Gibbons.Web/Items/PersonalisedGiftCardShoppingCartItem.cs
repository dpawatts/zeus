using System;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "[Internal Use Only]")]
	public class PersonalisedGiftCardShoppingCartItem : GiftCardShoppingCartItem
	{
		public override string ProductTitle
		{
			get { return "Personalized Gift Card"; }
		}
	}
}
