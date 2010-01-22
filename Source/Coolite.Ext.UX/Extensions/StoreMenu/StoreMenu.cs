using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using Ext.Net;

[assembly: WebResource("Coolite.Ext.UX.Extensions.StoreMenu.resources.ext.ux.menu.storemenu-min.js", "text/javascript")]
[assembly: WebResource("Coolite.Ext.UX.Extensions.StoreMenu.resources.ext.ux.menu.storemenu.js", "text/javascript")]
namespace Coolite.Ext.UX
{
	[Designer(typeof(EmptyDesigner))]
	[DefaultProperty("")]
	//[ToolboxBitmap(typeof(StoreMenu), "Extensions.GMapPanel.GMapPanel.bmp")]
	[ToolboxData("<{0}:StoreMenu runat=\"server\"></{0}:StoreMenu>")]
	[Description("Store Menu")]
	public class StoreMenu : Menu
	{
		protected override List<ResourceItem> Resources
		{
			get
			{
				List<ResourceItem> baseList = base.Resources;
				baseList.Capacity += 1;

				baseList.Add(new ClientStyleItem(typeof(StoreMenu), "Coolite.Ext.UX.Extensions.StoreMenu.resources.ext.ux.menu.storemenu-min.js", "ux/extensions/storemenu/storemenu.js"));

				return baseList;
			}
		}

		public override string InstanceOf
		{
			get { return "storemenu"; }
		}

		public override string XType
		{
			get { return "Ext.ux.menu.StoreMenu"; }
		}
	}
}