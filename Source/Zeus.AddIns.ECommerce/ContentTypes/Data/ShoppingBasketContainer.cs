using Ext.Net;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Shopping Basket Container")]
	[RestrictParents(typeof(Shop))]
	public class ShoppingBasketContainer : BaseContentItem
	{
		protected override Icon Icon
		{
			get { return Icon.BasketGo; }
		}
	}
}