using System.Linq;
using Coolite.Ext.Web;

namespace Zeus.Admin.ActionPlugins
{
	public class LanguageOverviewActionPlugin : LanguageActionPluginBase
	{
		public override int SortOrder
		{
			get { return 1; }
		}

		public override MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Language Overview",
				IconUrl = Utility.GetCooliteIconUrl(Icon.WorldGo)
			};

			menuItem.Handler = string.Format("function() {{ zeus.reloadContentPanel('Language Overview', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.LanguageOverview.aspx") + "?selected=" + contentItem.Path);

			return menuItem;
		}
	}
}