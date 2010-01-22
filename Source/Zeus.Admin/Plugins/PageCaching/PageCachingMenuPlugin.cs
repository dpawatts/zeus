using Coolite.Ext.Web;
using Zeus.Security;

namespace Zeus.Admin.Plugins.PageCaching
{
	[ActionPluginGroup("Caching", 40)]
	public class PageCachingMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		public override string GroupName
		{
			get { return "Caching"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Administer; }
		}

		public override int SortOrder
		{
			get { return 1; }
		}

		public override bool IsApplicable(ContentItem contentItem)
		{
			if (!contentItem.IsPage)
				return false;

			return base.IsApplicable(contentItem);
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ top.zeus.reloadContentPanel('Page Caching', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.Plugins.PageCaching.Default.aspx") + "?selected=" + contentItem.Path);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Page Caching",
				IconUrl = Utility.GetCooliteIconUrl(Icon.Package),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}