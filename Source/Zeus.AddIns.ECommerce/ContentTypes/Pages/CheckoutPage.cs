using Ext.Net;
using Zeus.ContentProperties;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType("Checkout Page")]
	[RestrictParents(typeof(Shop))]
	public class CheckoutPage : BasePage
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.MoneyPound); }
		}

		[XhtmlStringContentProperty("Extra Information", 210)]
		public string ExtraInformation
		{
			get { return GetDetail("ExtraInformation", string.Empty); }
			set { SetDetail("ExtraInformation", value); }
		}
	}
}