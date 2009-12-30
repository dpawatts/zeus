using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins
{
	public interface IMainInterfacePlugin
	{
		string[] RequiredUserControls { get; }

		void ModifyInterface(IMainInterface mainInterface);
		void RegisterScripts(ScriptManager scriptManager);
		void RegisterStyles(ScriptManager scriptManager);
	}
}