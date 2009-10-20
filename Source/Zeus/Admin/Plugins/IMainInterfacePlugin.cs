using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins
{
	public interface IMainInterfacePlugin
	{
		void ModifyInterface(IMainInterface mainInterface);
		void RegisterScripts(ScriptManager scriptManager);
	}
}