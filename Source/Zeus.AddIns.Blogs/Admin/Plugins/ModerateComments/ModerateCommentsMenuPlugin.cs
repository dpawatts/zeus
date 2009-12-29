using Coolite.Ext.Web;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Admin;
using Zeus.Admin.Plugins;

namespace Zeus.AddIns.Blogs.Admin.Plugins.ModerateComments
{
	[ActionPluginGroup("Blog", 200)]
	public class ModerateCommentsMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		public override string GroupName
		{
			get { return "Blog"; }
		}

		public override int SortOrder
		{
			get { return 3; }
		}

		public override bool IsApplicable(ContentItem contentItem)
		{
			// Hide if this is not the Blog node
			if (!(contentItem is Blog))
				return false;

			return base.IsApplicable(contentItem);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
				{
					Text = "Moderate Comments",
					IconUrl = Utility.GetCooliteIconUrl(Icon.Comments),
					Handler = string.Format("function() {{ zeus.reloadContentPanel('Moderate Comments', '{0}'); }}",
						GetPageUrl(GetType(), "Zeus.AddIns.Blogs.Admin.Plugins.ModerateComments.Default.aspx") + "?selected=" + contentItem.Path)
				};

			return menuItem;
		}
	}
}