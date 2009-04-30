using System;
using System.Linq;
using System.Web.UI.WebControls;
using Isis.Web.Hosting;
using Zeus.AddIns.Mailouts.Services;
using Zeus.Admin;
using Zeus.AddIns.Mailouts.ContentTypes;
using Zeus.Web;

[assembly: EmbeddedResourceFile("Zeus.AddIns.Mailouts.Admin.Send.aspx", "Zeus.AddIns.Mailouts.Admin")]
namespace Zeus.AddIns.Mailouts.Admin
{
	[ActionPluginGroup("Mailout", 200)]
	[SendActionPluginAttribute]
	public partial class Send : PreviewFrameAdminPage
	{
		protected void btnSend_Click(object sender, EventArgs e)
		{
			Zeus.Context.Current.Resolve<IMailoutService>().Send((Campaign) SelectedItem);
			Refresh(SelectedItem, AdminFrame.Both, false,
				EmbeddedResourceUtility.GetUrl(typeof(Send).Assembly, "Zeus.AddIns.Mailouts.Admin.Sent.aspx") + "?selected=" + SelectedItem.Path);
		}

		protected int GetRecipientCount()
		{
			return Zeus.Context.Current.Resolve<IMailoutService>().GetRecipients((Campaign) SelectedItem).Count();
		}

		public class SendActionPluginAttribute : ActionPluginAttribute
		{
			public SendActionPluginAttribute()
				: base("Send", "Send", null, "Mailout", 1, null, "Zeus.AddIns.Mailouts.Admin.Send.aspx", "selected={selected}", Targets.Preview, "Zeus.AddIns.Mailouts.Resources.email_go.png")
			{
				TypeFilter = typeof(Campaign);
			}

			protected override ActionPluginState GetStateInternal(ContentItem contentItem, IWebContext webContext)
			{
				return ((Campaign) contentItem).Status == CampaignStatus.Sent 
					? ActionPluginState.Disabled
					: ActionPluginState.Enabled;
			}
		}

		protected string GetRecipientsUrl()
		{
			return EmbeddedResourceUtility.GetUrl(typeof(Send).Assembly, "Zeus.AddIns.Mailouts.Admin.CampaignRecipients.aspx")
				+ "?selected=" + SelectedItem.Path;
		}
	}
}
