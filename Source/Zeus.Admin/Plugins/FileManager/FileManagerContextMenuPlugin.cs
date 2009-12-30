using Coolite.Ext.Web;
using Zeus.FileSystem;
using Zeus.Security;

namespace Zeus.Admin.Plugins.FileManager
{
	public class FileManagerContextMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		public override string GroupName
		{
			get { return "NewEditDelete"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Administer; }
		}

		public override int SortOrder
		{
			get { return 4; }
		}

		public override bool IsApplicable(ContentItem contentItem)
		{
			if (!(contentItem is Folder))
				return false;

			return base.IsApplicable(contentItem);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "File Manager",
				IconUrl = Utility.GetCooliteIconUrl(Icon.Folder),
				Handler = "function() {{ fileManager.show(); }}"
			};

			return menuItem;
		}
	}
}