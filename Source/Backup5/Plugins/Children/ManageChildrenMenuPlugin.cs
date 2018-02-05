using Ext.Net;
using Zeus.Security;

namespace Zeus.Admin.Plugins.Children
{
	public class ManageChildrenMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		protected override bool AvailableByDefault
		{
			get { return false; }
		}

		public override string GroupName
		{
			get { return "NewEditDelete"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Read; }
		}

		public override int SortOrder
		{
			get { return 4; }
		}

		public override bool IsDefault(ContentItem contentItem)
		{
			return (contentItem is DataContentItem);
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ top.zeus.reloadContentPanel('Manage Children', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.Plugins.Children.Default.aspx") + "?selected=" + contentItem.Path);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Manage Children",
				IconUrl = Utility.GetCooliteIconUrl(Icon.SitemapColor),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}