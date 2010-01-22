using Ext.Net;
using Zeus.Configuration;
using Zeus.Security;

namespace Zeus.Admin.Plugins.Versions
{
	public class ListVersionsMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		public override string GroupName
		{
			get { return "ViewPreview"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Version; }
		}

		public override int SortOrder
		{
			get { return 4; }
		}

		public override bool IsApplicable(ContentItem contentItem)
		{
			// Hide, if versioning is disabled.
			if (!Context.Current.Resolve<AdminSection>().Versioning.Enabled)
				return false;

			return base.IsApplicable(contentItem);
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ zeus.reloadContentPanel('Versions', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.Plugins.Versions.Default.aspx") + "?selected=" + contentItem.Path);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Versions",
				IconUrl = Utility.GetCooliteIconUrl(Icon.BookPrevious),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}