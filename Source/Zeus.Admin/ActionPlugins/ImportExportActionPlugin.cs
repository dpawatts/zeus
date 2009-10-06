using Coolite.Ext.Web;
using Zeus.Configuration;
using Zeus.Security;

namespace Zeus.Admin.ActionPlugins
{
	public class ImportExportPlugin : ActionPluginBase
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

		public override MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Import / Export",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PackageGo)
			};

			menuItem.Handler = string.Format("function() {{ zeus.reloadContentPanel('Import / Export', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.ImportExport.Default.aspx") + "?selected=" + contentItem.Path);

			return menuItem;
		}
	}
}