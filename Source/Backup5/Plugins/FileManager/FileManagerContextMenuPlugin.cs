using Ext.Net;
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

		public override bool IsDefault(ContentItem contentItem)
		{
			return (contentItem is Folder);
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return "function() {{ fileManager.show(); }}";
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "File Manager",
				IconUrl = Utility.GetCooliteIconUrl(Icon.Folder),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}