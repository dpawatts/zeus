using Ext.Net;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Order Container")]
	[RestrictParents(typeof(Shop))]
	public class OrderContainer : BaseContentItem
	{
		public override string IconUrl
		{
			get { return GetIconUrl(Icon.BasketGo); }
		}
	}
}