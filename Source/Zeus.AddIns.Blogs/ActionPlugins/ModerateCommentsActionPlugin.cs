using Coolite.Ext.Web;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Admin;
using Zeus.Security;

namespace Zeus.AddIns.Blogs.ActionPlugins
{
	public class ModerateCommentsActionPlugin : ActionPluginBase
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

		public override MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Moderate Comments",
				IconUrl = Utility.GetCooliteIconUrl(Icon.Comments)
			};

			menuItem.Handler = string.Format("function() {{ zeus.reloadContentPanel('Moderate Comments', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.AddIns.Blogs.Admin.ModerateComments.aspx") + "?selected=" + contentItem.Path);

			return menuItem;
		}
	}
}