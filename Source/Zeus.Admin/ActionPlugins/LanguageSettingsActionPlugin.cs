using System.Linq;
using Coolite.Ext.Web;

namespace Zeus.Admin.ActionPlugins
{
	public class LanguageSettingsActionPlugin : LanguageActionPluginBase
	{
		public override int SortOrder
		{
			get { return 3; }
		}

		public override MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Language Settings",
				IconUrl = Utility.GetCooliteIconUrl(Icon.WorldEdit)
			};

			menuItem.Handler = string.Format("function() {{ zeus.reloadContentPanel('Language Settings', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.LanguageSettings.aspx") + "?selected=" + contentItem.Path);

			return menuItem;
		}
	}
}