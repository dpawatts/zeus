using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Delivery Method Container")]
	[RestrictParents(typeof(Shop))]
	public class DeliveryMethodContainer : BaseContentItem
	{
		public override string IconUrl
		{
			get { return GetIconUrl(typeof(DeliveryMethodContainer), "Zeus.AddIns.ECommerce.Icons.lorry_go.png"); }
		}
	}
}