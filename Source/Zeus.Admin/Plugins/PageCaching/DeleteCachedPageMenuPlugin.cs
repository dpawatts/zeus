using Ext.Net;
using Zeus.Security;
using Zeus.Web.Caching;

namespace Zeus.Admin.Plugins.PageCaching
{
	public class DeleteCachedPageMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		public override string GroupName
		{
			get { return "Caching"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Administer; }
		}

		public override string[] RequiredUserControls
		{
			get { return new[] { GetPageUrl(GetType(), "Zeus.Admin.Plugins.PageCaching.PageCachingUserControl.ascx") }; }
		}

		public override int SortOrder
		{
			get { return 2; }
		}

		public override bool IsApplicable(ContentItem contentItem)
		{
			if (!contentItem.IsPage)
				return false;

			return base.IsApplicable(contentItem);
		}

		public override bool IsEnabled(ContentItem contentItem)
		{
			if (!Context.Current.Resolve<ICachingService>().IsPageCached(contentItem))
				return false;

			return base.IsEnabled(contentItem);
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format(
				"function() {{ stbStatusBar.showBusy(); Coolite.AjaxMethods.PageCaching.DeleteCachedPage({0}, {{ url: '/admin/default.aspx', success: function() {{ stbStatusBar.setStatus({{ text: 'Deleted cached page', iconCls: '', clear: true }}); }} }}); }}",
				contentItem.ID);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Delete Cached Page",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PackageDelete),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}