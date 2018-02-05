using Ext.Net;
using Zeus.Security;

namespace Zeus.Admin.Plugins.Permissions
{
	public class ManagePermissionsMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		protected override bool AvailableByDefault
		{
			get { return false; }
		}

		public override string GroupName
		{
			get { return "ViewPreview"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Administer; }
		}

		public override int SortOrder
		{
			get { return 3; }
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ top.zeus.reloadContentPanel('Permissions', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.Plugins.Permissions.Default.aspx") + "?selected=" + contentItem.Path);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Permissions",
				IconUrl = Utility.GetCooliteIconUrl(Icon.Lock),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}