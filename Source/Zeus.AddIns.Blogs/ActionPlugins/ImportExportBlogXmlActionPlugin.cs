using Coolite.Ext.Web;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Admin;
using Zeus.Security;

namespace Zeus.AddIns.Blogs.ActionPlugins
{
	public class ImportExportBlogXmlActionPlugin : ActionPluginBase
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

		public override MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Import Atom XML",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PackageIn)
			};

			menuItem.Handler = string.Format("function() {{ zeus.reloadContentPanel('Import Atom XML', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.AddIns.Blogs.Plugins.ImportAtom.aspx") + "?selected=" + contentItem.Path);

			return menuItem;
		}
	}
}