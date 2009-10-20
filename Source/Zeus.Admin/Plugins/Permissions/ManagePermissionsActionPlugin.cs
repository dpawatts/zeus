using Coolite.Ext.Web;
using Zeus.Security;

namespace Zeus.Admin.Plugins.Permissions
{
	public class ManagePermissionsMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
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

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Permissions",
				IconUrl = Utility.GetCooliteIconUrl(Icon.Lock),
				Handler = string.Format("function() {{ zeus.reloadContentPanel('Permissions', '{0}'); }}",
					GetPageUrl(GetType(), "Zeus.Admin.Plugins.Permissions.Default.aspx") + "?selected=" + contentItem.Path)
			};

			return menuItem;
		}
	}
}