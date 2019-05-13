using Ext.Net;
using Zeus.Security;

namespace Zeus.Admin.Plugins.DeleteItem
{
	public class DeleteItemMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		public override string GroupName
		{
			get { return "NewEditDelete"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Delete; }
		}

		public override string[] RequiredUserControls
		{
			get { return new[] { GetPageUrl(GetType(), "Zeus.Admin.Plugins.DeleteItem.DeleteUserControl.ascx") }; }
		}

		public override int SortOrder
		{
			get { return 3; }
		}

		public override bool IsEnabled(ContentItem contentItem)
		{
			if (Context.UrlParser.IsRootOrStartPage(contentItem))
				return false;

			return base.IsEnabled(contentItem);
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format(@"
				function()
				{{
					Ext.net.DirectMethods.Delete.ShowDialog('Delete Item',
						'<b>Are you sure you wish to delete this item?</b><br /><img src=\'{0}\' /> {1}',
						'{2}', {{ url : '{3}' }});
				}}",
				contentItem.IconUrl, contentItem.Title.Replace("'", "\\'"), contentItem.ID,
				Context.AdminManager.GetAdminDefaultUrl());
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Delete",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PageDelete),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}