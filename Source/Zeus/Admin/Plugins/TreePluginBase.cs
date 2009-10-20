using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins
{
	public abstract class TreePluginBase : ITreePlugin
	{
		public virtual string[] RequiredScripts
		{
			get { return null; }
		}

		public abstract void ModifyTree(TreePanel treePanel, IMainInterface mainInterface);
	}
}