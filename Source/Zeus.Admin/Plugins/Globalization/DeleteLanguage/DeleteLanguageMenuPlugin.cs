using System.Linq;
using Ext.Net;

namespace Zeus.Admin.Plugins.Globalization.DeleteLanguage
{
	public class DeleteLanguageMenuPlugin : LanguageMenuPluginBase, IContextMenuPlugin
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

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ zeus.reloadContentPanel('Delete Language', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.Plugins.Globalization.DeleteLanguage.Default.aspx") + "?selected=" +
				contentItem.Path);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Delete Language",
				IconUrl = Utility.GetCooliteIconUrl(Icon.WorldDelete),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}