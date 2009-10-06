using Coolite.Ext.Web;
using Zeus.Security;

namespace Zeus.Admin.ActionPlugins
{
	public class PermissionsActionPlugin : ActionPluginBase
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

		public override MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Permissions",
				IconUrl = Utility.GetCooliteIconUrl(Icon.Lock)
			};

			menuItem.Handler = string.Format("function() {{ zeus.reloadContentPanel('Permissions', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.Security.Default.aspx") + "?selected=" + contentItem.Path);

			return menuItem;
		}
	}
}