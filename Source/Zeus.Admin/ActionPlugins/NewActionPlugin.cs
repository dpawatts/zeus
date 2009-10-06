using System.Linq;
using Coolite.Ext.Web;
using Zeus.ContentTypes;
using Zeus.Security;

namespace Zeus.Admin.ActionPlugins
{
	public class NewActionPlugin : ActionPluginBase
	{
		public override string GroupName
		{
			get { return "NewEditDelete"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Create; }
		}

		public override int SortOrder
		{
			get { return 1; }
		}

		public override bool IsEnabled(ContentItem contentItem)
		{
			if (!base.IsEnabled(contentItem))
				return false;

			// Check that this content item has allowed children
			IContentTypeManager contentTypeManager = Context.ContentTypes;
			ContentType contentType = contentTypeManager.GetContentType(contentItem.GetType());
			if (!contentTypeManager.GetAllowedChildren(contentType, null, Context.Current.WebContext.User).Any())
				return false;

			return true;
		}

		public override MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "New",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PageAdd)
			};

			menuItem.Handler = string.Format("function() {{ zeus.reloadContentPanel('New', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.New.aspx") + "?selected=" + contentItem.Path);

			// TODO: Add child menu items for types that can be created under the current item.

			return menuItem;
		}
	}
}