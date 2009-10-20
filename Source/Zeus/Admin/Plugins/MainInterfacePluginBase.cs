using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins
{
	public abstract class MainInterfacePluginBase : IMainInterfacePlugin
	{
		public virtual void ModifyInterface(IMainInterface mainInterface)
		{
			
		}

		public virtual void RegisterScripts(ScriptManager scriptManager)
		{
			
		}
	}
}