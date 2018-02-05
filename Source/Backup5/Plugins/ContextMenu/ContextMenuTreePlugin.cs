using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using Zeus.Web.Hosting;

namespace Zeus.Admin.Plugins.ContextMenu
{
	public class ContextMenuTreePlugin : TreePluginBase
	{
		public override void ModifyTree(TreePanel treePanel, IMainInterface mainInterface)
		{
			treePanel.Listeners.ContextMenu.Handler = string.Format(@"function(node, e)
				{{
                    if (node.text == undefined || (!!node.ui && !!node.ui.elNode && jQuery(node.ui.elNode).hasClass('disable-context'))) {{
                        return false;
                    }}
					{0}.selectPath(node.getPath());
					var contextMenu = new Ext.ux.menu.StoreMenu(
					{{
						url: '{1}',
						baseParams: {{ node: node.id }},
						width: 'auto'
					}});
					
					contextMenu.showAt(e.getXY());
				}}",
				treePanel.ClientID, Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(GetType().Assembly, "Zeus.Admin.Plugins.ContextMenu.ContextMenuLoader.ashx"));
		}

		public override void ModifyTreeNode(TreeNodeBase treeNode, ContentItem contentItem)
		{
			foreach (ActionPluginGroupAttribute actionPluginGroup in Context.AdminManager.GetActionPluginGroups())
				foreach (IContextMenuPlugin plugin in GetPlugins(actionPluginGroup.Name))
					if (plugin.IsApplicable(contentItem) && (plugin.IsEnabled(contentItem) && plugin.IsDefault(contentItem)))
					{
						treeNode.Href = "javascript:(" + plugin.GetJavascriptHandler(contentItem) + ")();";
						return;
					}
		}

		private static IEnumerable<IContextMenuPlugin> GetPlugins(string groupName)
		{
			return Context.Current.ResolveAll<IContextMenuPlugin>()
				.Where(p => p.GroupName == groupName)
				.OrderBy(p => p.SortOrder);
		}
	}
}