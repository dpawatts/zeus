using System.ComponentModel;
using System.Web.UI;
using Coolite.Ext.Web;

[assembly: WebResource("Coolite.Ext.UX.Extensions.StoreMenu.resources.ext.ux.menu.storemenu-min.js", "text/javascript")]
[assembly: WebResource("Coolite.Ext.UX.Extensions.StoreMenu.resources.ext.ux.menu.storemenu.js", "text/javascript")]

namespace Coolite.Ext.UX
{
	[Designer(typeof(EmptyDesigner))]
	[DefaultProperty("")]
	[Xtype("storemenu")]
	[InstanceOf(ClassName = "Ext.ux.GMapPanel")]
	[ClientScript(Type = typeof(StoreMenu), WebResource = "Coolite.Ext.UX.Extensions.StoreMenu.resources.ext.ux.menu.storemenu-min.js", FilePath = "ux/extensions/storemenu/storemenu.js")]
	//[ToolboxBitmap(typeof(StoreMenu), "Extensions.GMapPanel.GMapPanel.bmp")]
	[ToolboxData("<{0}:StoreMenu runat=\"server\"></{0}:StoreMenu>")]
	[Description("Store Menu")]
	public class StoreMenu : Menu
	{
		
	}
}