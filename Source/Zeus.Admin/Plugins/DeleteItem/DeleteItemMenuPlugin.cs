using System.Web.UI;
using Ext.Net;
using Zeus.BaseLibrary.Web.UI;
using Zeus.Security;

[assembly: WebResource("Zeus.Admin.Plugins.DeleteItem.Resources.Ext.ux.zeus.DeleteConfirmation.js", "text/javascript")]

namespace Zeus.Admin.Plugins.DeleteItem
{
	public class DeleteItemMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		public override string GroupName
		{
			get { return "NewEditDelete"; }
		}

		public override string[] RequiredScripts
		{
			get { return new[] { WebResourceUtility.GetUrl(GetType(), "Zeus.Admin.Plugins.DeleteItem.Resources.Ext.ux.zeus.DeleteConfirmation.js") }; }
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
			return string.Format("function() {{ top.Ext.ux.zeus.DeleteConfirmation.show('{0}', '{1}', '{2}'); }}",
				contentItem.Title.Replace("'", "\\'"), contentItem.ID, contentItem.IconUrl);
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