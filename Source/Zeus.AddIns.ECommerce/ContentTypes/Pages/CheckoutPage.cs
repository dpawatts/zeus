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
			get { return GetIconUrl(typeof(CheckoutPage), "Zeus.AddIns.ECommerce.Icons.money_pound.png"); }
		}
	}
}