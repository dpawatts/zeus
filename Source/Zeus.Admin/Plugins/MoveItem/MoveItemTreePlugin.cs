using Ext.Net;

namespace Zeus.Admin.Plugins.MoveItem
{
	public class MoveItemTreePlugin : TreePluginBase
	{
		public override string[] RequiredUserControls
		{
			get { return new[] { GetPageUrl(GetType(), "Zeus.Admin.Plugins.MoveItem.MoveUserControl.ascx") }; }
		}

		public override void ModifyTree(TreePanel treePanel, IMainInterface mainInterface)
		{
			treePanel.Listeners.MoveNode.Handler = string.Format(@"
				{0}.showBusy();
				Ext.net.DirectMethods.Move.MoveNode(node.id, newParent.id, index,
				{{
					url: '{1}',
					success: function() {{ {0}.setStatus({{ text: 'Moved item', iconCls: '', clear: true }}); }}
				}})",
				mainInterface.StatusBar.ClientID, Context.AdminManager.GetAdminDefaultUrl());
		}
	}
}