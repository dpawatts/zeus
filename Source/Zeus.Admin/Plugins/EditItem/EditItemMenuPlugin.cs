using Coolite.Ext.Web;
using Zeus.Security;

namespace Zeus.Admin.Plugins.EditItem
{
	public class EditItemMenuPlugin : MenuPluginBase, IContextMenuPlugin
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

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Edit",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PageEdit),
				Handler = string.Format("function() {{ zeus.reloadContentPanel('Edit', '{0}'); }}",
					GetPageUrl(GetType(), "Zeus.Admin.Plugins.EditItem.Default.aspx") + "?selected=" + contentItem.Path)
			};

			return menuItem;
		}
	}
}