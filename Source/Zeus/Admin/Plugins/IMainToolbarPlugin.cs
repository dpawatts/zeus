using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins
{
	public interface IMainToolbarPlugin : IToolbarPlugin
	{
		ToolbarButton GetToolbarButton();
		bool IsEnabled();
	}
}