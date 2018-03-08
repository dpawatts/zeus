using Ext.Net;
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

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ top.zeus.reloadContentPanel('Edit', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.Plugins.EditItem.Default.aspx") + "?selected=" + contentItem.Path);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Edit",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PageEdit),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}