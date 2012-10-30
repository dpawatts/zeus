using System;
using System.Linq;
using Zeus.AddIns.Mailouts.Services;
using Zeus.Admin;
using Zeus.AddIns.Mailouts.ContentTypes;
using Zeus.Web.Hosting;

namespace Zeus.AddIns.Mailouts.Admin
{
	[ActionPluginGroup("Mailout", 200)]
	public partial class Send : PreviewFrameAdminPage
	{
		protected void btnSend_Click(object sender, EventArgs e)
		{
			Zeus.Context.Current.Resolve<IMailoutService>().Send((Campaign) SelectedItem);
			Refresh(SelectedItem, AdminFrame.Both, false,
				Engine.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(typeof(Send).Assembly, "Zeus.AddIns.Mailouts.Admin.Sent.aspx") + "?selected=" + SelectedItem.Path);
		}

		protected int GetRecipientCount()
		{
			return Zeus.Context.Current.Resolve<IMailoutService>().GetRecipients((Campaign) SelectedItem).Count();
		}

		protected string GetRecipientsUrl()
		{
			return Engine.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(typeof(Send).Assembly, "Zeus.AddIns.Mailouts.Admin.CampaignRecipients.aspx")
				+ "?selected=" + SelectedItem.Path;
		}
	}
}
