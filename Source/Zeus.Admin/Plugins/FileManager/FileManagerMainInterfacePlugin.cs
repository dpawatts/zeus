using System.Web.UI;
using Zeus.BaseLibrary.Web.UI;

[assembly: WebResource("Zeus.Admin.Plugins.FileManager.Resources.Ext.ux.zeus.FileManager.css", "text/css")]
[assembly: WebResource("Zeus.Admin.Plugins.FileManager.Resources.Ext.ux.zeus.FileManager.js", "text/javascript")]

namespace Zeus.Admin.Plugins.FileManager
{
	public class FileManagerMainInterfacePlugin : MainInterfacePluginBase
	{
		public override string[] RequiredScripts
		{
			get { return new[] { WebResourceUtility.GetUrl(GetType(), "Zeus.Admin.Plugins.FileManager.Resources.Ext.ux.zeus.FileManager.js") }; }
		}

		public override string[] RequiredStyles
		{
			get { return new[] { WebResourceUtility.GetUrl(GetType(), "Zeus.Admin.Plugins.FileManager.Resources.Ext.ux.zeus.FileManager.css") }; }
		}

		public override string[] RequiredUserControls
		{
			get { return new[] { GetPageUrl(GetType(), "Zeus.Admin.Plugins.FileManager.FileManagerUserControl.ascx") }; }
		}
	}
}