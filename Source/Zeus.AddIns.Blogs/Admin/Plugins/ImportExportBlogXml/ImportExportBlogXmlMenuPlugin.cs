using Ext.Net;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Admin.Plugins;
using Zeus.Security;

namespace Zeus.AddIns.Blogs.Admin.Plugins.ImportExportBlogXml
{
	public class ImportExportBlogXmlMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		public override string GroupName
		{
			get { return "Blog"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.ImportExport; }
		}

		public override int SortOrder
		{
			get { return 2; }
		}

		public override bool IsApplicable(ContentItem contentItem)
		{
			// Hide if this is not the Blog node
			if (!(contentItem is Blog))
				return false;

			return base.IsApplicable(contentItem);
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ zeus.reloadContentPanel('Import Atom XML', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.AddIns.Blogs.Plugins.ImportAtom.aspx") + "?selected=" + contentItem.Path);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
				{
					Text = "Import Atom XML",
					IconUrl = Utility.GetCooliteIconUrl(Icon.PackageIn),
					Handler = GetJavascriptHandler(contentItem)
				};

			return menuItem;
		}
	}
}