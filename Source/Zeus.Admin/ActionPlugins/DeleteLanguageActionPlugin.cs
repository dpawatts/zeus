using System.Linq;
using Coolite.Ext.Web;

namespace Zeus.Admin.ActionPlugins
{
	public class DeleteLanguageActionPlugin : LanguageActionPluginBase
	{
		public override int SortOrder
		{
			get { return 2; }
		}

		public override bool IsEnabled(ContentItem contentItem)
		{
			// Disable, if there are no translations of the original item.
			if (!Context.Current.LanguageManager.GetTranslationsOf(contentItem.TranslationOf ?? contentItem, false).Any())
				return false;

			return base.IsEnabled(contentItem);
		}

		public override MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Delete Language",
				IconUrl = Utility.GetCooliteIconUrl(Icon.WorldDelete)
			};

			menuItem.Handler = string.Format("function() {{ zeus.reloadContentPanel('Delete Language', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.DeleteLanguage.aspx") + "?selected=" + contentItem.Path);

			return menuItem;
		}
	}
}