using System.Web.UI;
using Isis.Web.UI;
using ScriptManager=Coolite.Ext.Web.ScriptManager;

[assembly: WebResource("Zeus.Admin.Plugins.ContextMenu.Resources.Ext.ux.zeus.ContextMenuPlugin.js", "text/javascript")]

namespace Zeus.Admin.Plugins.ContextMenu
{
	public class ContextMenuMainInterfacePlugin : MainInterfacePluginBase
	{
		public override void ModifyInterface(IMainInterface mainInterface)
		{
			foreach (IContextMenuPlugin plugin in Context.Current.ResolveAll<IContextMenuPlugin>())
			{
				string[] requiredUserControls = plugin.RequiredUserControls;
				if (requiredUserControls != null)
					mainInterface.LoadUserControls(requiredUserControls);
			}
		}

		public override void RegisterScripts(ScriptManager scriptManager)
		{
			// Render action plugin scripts.
			scriptManager.RegisterClientScriptInclude("ActionPlugin",
				WebResourceUtility.GetUrl(typeof(ContextMenuMainInterfacePlugin), "Zeus.Admin.Plugins.ContextMenu.Resources.Ext.ux.zeus.ContextMenuPlugin.js"));

			foreach (IContextMenuPlugin plugin in Context.Current.ResolveAll<IContextMenuPlugin>())
			{
				string[] requiredScripts = plugin.RequiredScripts;
				if (requiredScripts != null)
					foreach (string requiredScript in requiredScripts)
						scriptManager.RegisterClientScriptInclude(plugin.GetType().FullName, requiredScript);
			}
		}
	}
}