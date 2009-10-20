using Coolite.Ext.Web;
using Zeus.Web.Hosting;

namespace Zeus.Admin.Plugins.ContextMenu
{
	public class ContextMenuTreePlugin : TreePluginBase
	{
		public override void ModifyTree(TreePanel treePanel, IMainInterface mainInterface)
		{
			treePanel.Listeners.ContextMenu.Handler = string.Format("function(node, e) {{ {0}.selectPath(node.getPath()); var contextMenu = new Ext.ux.menu.StoreMenu({{ url: '{1}', baseParams: {{ node: node.id }}, width: 'auto' }}); contextMenu.showAt(e.getXY()); }}",
				treePanel.ClientID, Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(GetType().Assembly, "Zeus.Admin.Plugins.ContextMenu.ContextMenuLoader.ashx"));
		}
	}
}