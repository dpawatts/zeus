using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins.CopyItem
{
	public class CopyItemMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		public override string GroupName
		{
			get { return "CutCopyPaste"; }
		}

		public override int SortOrder
		{
			get { return 2; }
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ zeus.memorize('{0}', '{1}'); }}",
				contentItem.Path, GetPageUrl(GetType(), "Zeus.Admin.Plugins.CopyItem.Default.aspx"));
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Copy",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PageCopy),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}