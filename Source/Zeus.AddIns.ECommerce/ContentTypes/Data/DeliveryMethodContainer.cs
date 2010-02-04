using Ext.Net;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Delivery Method Container")]
	[RestrictParents(typeof(Shop))]
	public class DeliveryMethodContainer : BaseContentItem
	{
		protected override Icon Icon
		{
			get { return Icon.LorryGo; }
		}
	}
}