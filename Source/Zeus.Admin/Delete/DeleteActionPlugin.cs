using System.Web.UI;
using Coolite.Ext.Web;
using Isis.Web.UI;
using Zeus.Security;

[assembly: WebResource("Zeus.Admin.Delete.Resources.Ext.ux.zeus.DeleteConfirmation.js", "text/javascript")]

namespace Zeus.Admin.Delete
{
	public class DeleteActionPlugin : ActionPluginBase
	{
		public override string GroupName
		{
			get { return "NewEditDelete"; }
		}

		public override string[] RequiredScripts
		{
			get { return new[] { WebResourceUtility.GetUrl(GetType(), "Zeus.Admin.Delete.Resources.Ext.ux.zeus.DeleteConfirmation.js") }; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Delete; }
		}

		public override string[] RequiredUserControls
		{
			get { return new[] { GetPageUrl(GetType(), "Zeus.Admin.Delete.DeleteUserControl.ascx") }; }
		}

		public override int SortOrder
		{
			get { return 3; }
		}

		public override bool IsEnabled(ContentItem contentItem)
		{
			if (Context.UrlParser.IsRootOrStartPage(contentItem))
				return false;

			return base.IsEnabled(contentItem);
		}

		public override MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Delete",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PageDelete)
			};

			menuItem.Handler = string.Format("function() {{ Ext.ux.zeus.DeleteConfirmation.show('{0}', '{1}', '{2}'); }}",
				contentItem.Title, contentItem.ID, contentItem.IconUrl);

			return menuItem;
		}
	}
}