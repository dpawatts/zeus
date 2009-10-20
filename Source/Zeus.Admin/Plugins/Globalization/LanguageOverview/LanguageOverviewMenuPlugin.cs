using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins.Globalization.LanguageOverview
{
	public class LanguageOverviewMenuPlugin : LanguageMenuPluginBase, IContextMenuPlugin
	{
		public override int SortOrder
		{
			get { return 1; }
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Language Overview",
				IconUrl = Utility.GetCooliteIconUrl(Icon.WorldGo),
				Handler = string.Format("function() {{ zeus.reloadContentPanel('Language Overview', '{0}'); }}",
					GetPageUrl(GetType(), "Zeus.Admin.Plugins.Globalization.LanguageOverview.Default.aspx") + "?selected=" + contentItem.Path)
			};

			return menuItem;
		}
	}
}