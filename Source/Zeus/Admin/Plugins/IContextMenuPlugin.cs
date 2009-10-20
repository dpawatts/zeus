using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins
{
	public interface IContextMenuPlugin
	{
		string GroupName { get; }
		string[] RequiredScripts { get; }
		string[] RequiredUserControls { get; }
		int SortOrder { get; }

		bool IsApplicable(ContentItem contentItem);
		bool IsEnabled(ContentItem contentItem);
		MenuItem GetMenuItem(ContentItem contentItem);
	}
}