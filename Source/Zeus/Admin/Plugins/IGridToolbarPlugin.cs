using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins
{
	public interface IGridToolbarPlugin : IToolbarPlugin
	{
		ToolbarButton GetToolbarButton(ContentItem contentItem, GridPanel gridPanel);
		void ModifyGrid(ToolbarButton button, GridPanel gridPanel);
		bool IsEnabled(ContentItem contentItem);
	}
}