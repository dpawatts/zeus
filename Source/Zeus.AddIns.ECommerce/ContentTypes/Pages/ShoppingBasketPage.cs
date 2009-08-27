using Zeus.ContentProperties;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType("Shopping Basket Page")]
	[RestrictParents(typeof(Shop))]
	public class ShoppingBasketPage : BasePage
	{
		public override string IconUrl
		{
			get { return GetIconUrl(typeof(ShoppingBasketPage), "Zeus.AddIns.ECommerce.Icons.basket.png"); }
		}

		[XhtmlStringContentProperty("Extra Information", 210)]
		public string ExtraInformation
		{
			get { return GetDetail("ExtraInformation", string.Empty); }
			set { SetDetail("ExtraInformation", value); }
		}
	}
}