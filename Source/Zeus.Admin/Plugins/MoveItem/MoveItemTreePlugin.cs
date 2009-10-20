using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins.MoveItem
{
	public class MoveItemTreePlugin : TreePluginBase
	{
		public override void ModifyTree(TreePanel treePanel, IMainInterface mainInterface)
		{
			treePanel.Listeners.MoveNode.Handler = string.Format("{0}.showBusy(); Coolite.AjaxMethods.Move.MoveNode(node.id, newParent.id, index, {{ url: '/admin/default.aspx' }})",
				mainInterface.StatusBar.ClientID);
		}
	}
}