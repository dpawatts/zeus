using Coolite.Ext.Web;
using Zeus.Security;

namespace Zeus.Admin.Plugins.Children
{
	public class ManageChildrenMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
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

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Manage Children",
				IconUrl = Utility.GetCooliteIconUrl(Icon.ApplicationViewDetail),
				Handler = string.Format("function() {{ top.zeus.reloadContentPanel('Manage Children', '{0}'); }}",
					GetPageUrl(GetType(), "Zeus.Admin.Plugins.Children.Default.aspx") + "?selected=" + contentItem.Path)
			};

			return menuItem;
		}
	}
}