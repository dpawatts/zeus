using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins
{
	public interface ITreePlugin
	{
		string[] RequiredScripts { get; }
		void ModifyTree(TreePanel treePanel, IMainInterface mainInterface);
	}
}