using System.Web.UI;
using Ext.Net;

namespace Zeus.Admin.Plugins.ManageZones
{
	public class ManageZonesMainInterfacePlugin : MainInterfacePluginBase
	{
		public override void ModifyInterface(IMainInterface mainInterface)
		{
			ResourceManager.GetInstance((Page) mainInterface).RegisterIcon(Icon.Add);
			ResourceManager.GetInstance((Page) mainInterface).RegisterIcon(Icon.ApplicationSideBoxes);
		}
	}
}