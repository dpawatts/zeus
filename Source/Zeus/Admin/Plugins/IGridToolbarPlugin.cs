using Ext.Net;

namespace Zeus.Admin.Plugins
{
	public interface IGridToolbarPlugin : IToolbarPlugin
	{
		Button GetToolbarButton(ContentItem contentItem, GridPanel gridPanel);
		void ModifyGrid(Button button, GridPanel gridPanel);
		bool IsEnabled(ContentItem contentItem);
	}
}