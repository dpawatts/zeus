using Ext.Net;

namespace Zeus.Admin.Plugins
{
	public interface IMainInterfacePlugin
	{
		string[] RequiredUserControls { get; }

		void ModifyInterface(IMainInterface mainInterface);
		void RegisterScripts(ResourceManager scriptManager);
		void RegisterStyles(ResourceManager scriptManager);
	}
}