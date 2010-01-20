using Coolite.Ext.Web;
using Zeus.Configuration;
using Zeus.Security;

namespace Zeus.Admin.Plugins.ImportExport
{
	public class ImportExportMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		public override string GroupName
		{
			get { return "ViewPreview"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.ImportExport; }
		}

		public override int SortOrder
		{
			get { return 5; }
		}

		public override bool IsEnabled(ContentItem contentItem)
		{
			// Hide, if import / export is disabled.
			if (!Context.Current.Resolve<AdminSection>().ImportExportEnabled)
				return false;

			return base.IsEnabled(contentItem);
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ top.zeus.reloadContentPanel('Import / Export', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.Plugins.ImportExport.Default.aspx") + "?selected=" + contentItem.Path);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Import / Export",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PackageGo),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}