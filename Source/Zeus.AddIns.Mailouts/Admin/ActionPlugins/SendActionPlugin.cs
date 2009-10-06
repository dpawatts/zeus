using Coolite.Ext.Web;
using Zeus.AddIns.Mailouts.ContentTypes;
using Zeus.Admin;

namespace Zeus.AddIns.Mailouts.Admin.ActionPlugins
{
	public class SendActionPlugin : ActionPluginBase
	{
		public override string GroupName
		{
			get { return "Mailout"; }
		}

		public override int SortOrder
		{
			get { return 1; }
		}

		public override bool IsApplicable(ContentItem contentItem)
		{
			// Hide if this is not the Campaign node
			if (!(contentItem is Campaign))
				return false;

			return base.IsApplicable(contentItem);
		}

		public override bool IsEnabled(ContentItem contentItem)
		{
			if (((Campaign)contentItem).Status == CampaignStatus.Sent)
				return false;

			return base.IsEnabled(contentItem);
		}

		public override MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Send",
				IconUrl = Utility.GetCooliteIconUrl(Icon.EmailGo)
			};

			menuItem.Handler = string.Format("function() {{ zeus.reloadContentPanel('Send', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.AddIns.Mailouts.Admin.Send.aspx") + "?selected=" + contentItem.Path);

			// TODO: Add child menu items for types that can be created under the current item.

			return menuItem;
		}
	}
}