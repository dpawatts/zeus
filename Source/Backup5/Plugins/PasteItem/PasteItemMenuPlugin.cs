using Ext.Net;

namespace Zeus.Admin.Plugins.PasteItem
{
	public class PasteItemMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		public override string GroupName
		{
			get { return "CutCopyPaste"; }
		}

		public override int SortOrder
		{
			get { return 3; }
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ zeus.reloadContentPanel('Paste', '{0}&memory={{memory}}&action={{action}}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.Plugins.PasteItem.Default.aspx") + "?selected=" + contentItem.Path);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Paste",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PagePaste),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}