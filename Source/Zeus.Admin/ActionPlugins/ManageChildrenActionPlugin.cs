using Coolite.Ext.Web;
using Zeus.Security;

namespace Zeus.Admin.ActionPlugins
{
	public class ManageChildrenPlugin : ActionPluginBase
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

		public override Coolite.Ext.Web.MenuItem GetMenuItem(ContentItem contentItem)
		{
			Coolite.Ext.Web.MenuItem menuItem = new Coolite.Ext.Web.MenuItem
			{
				Text = "Manage Children",
				IconUrl = Utility.GetCooliteIconUrl(Icon.ApplicationViewDetail)
			};

			menuItem.Handler = string.Format("function() {{ zeus.reloadContentPanel('Manage Children', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.Children.aspx") + "?selected=" + contentItem.Path);

			return menuItem;
		}
	}
}