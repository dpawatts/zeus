using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins.Globalization.LanguageSettings
{
	public class LanguageSettingsMenuPlugin : LanguageMenuPluginBase, IContextMenuPlugin
	{
		public override int SortOrder
		{
			get { return 3; }
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ zeus.reloadContentPanel('Language Settings', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.Plugins.Globalization.LanguageSettings.Default.aspx") + "?selected=" + contentItem.Path);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Language Settings",
				IconUrl = Utility.GetCooliteIconUrl(Icon.WorldEdit),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}