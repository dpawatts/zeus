using Ext.Net;

namespace Zeus.Admin.Plugins
{
	public interface IMainToolbarPlugin : IToolbarPlugin
	{
		Button GetToolbarButton();
		bool IsEnabled();
	}
}