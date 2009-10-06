using Coolite.Ext.Web;

namespace Zeus.Admin
{
	public interface IGridToolbarPlugin
	{
		string[] RequiredScripts { get; }
		string[] RequiredUserControls { get; }
		int SortOrder { get; }

		ToolbarButton GetToolbarButton(ContentItem contentItem, GridPanel gridPanel);
		void ModifyGrid(ToolbarButton button, GridPanel gridPanel);
		bool IsEnabled(ContentItem contentItem);
	}
}