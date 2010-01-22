using Ext.Net;

namespace Zeus.Admin.Plugins
{
	public interface ITreePlugin
	{
		string[] RequiredScripts { get; }
		string[] RequiredUserControls { get; }
		void ModifyTree(TreePanel treePanel, IMainInterface mainInterface);
		void ModifyTreeNode(TreeNodeBase treeNode, ContentItem contentItem);
	}
}