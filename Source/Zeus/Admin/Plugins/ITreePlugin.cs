using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins
{
	public interface ITreePlugin
	{
		string[] RequiredScripts { get; }
		string[] RequiredUserControls { get; }
		void ModifyTree(TreePanel treePanel, IMainInterface mainInterface);
	}
}