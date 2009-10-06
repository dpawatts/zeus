using Coolite.Ext.Web;
using Zeus.Security;

namespace Zeus.Admin.ActionPlugins
{
	public class EditActionPlugin : ActionPluginBase
	{
		public override string GroupName
		{
			get { return "NewEditDelete"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Change; }
		}

		public override int SortOrder
		{
			get { return 2; }
		}

		public override Coolite.Ext.Web.MenuItem GetMenuItem(ContentItem contentItem)
		{
			Coolite.Ext.Web.MenuItem menuItem = new Coolite.Ext.Web.MenuItem
			{
				Text = "Edit",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PageEdit)
			};

			menuItem.Handler = string.Format("function() {{ zeus.reloadContentPanel('Edit', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.Edit.aspx") + "?selected=" + contentItem.Path);

			return menuItem;
		}
	}
}