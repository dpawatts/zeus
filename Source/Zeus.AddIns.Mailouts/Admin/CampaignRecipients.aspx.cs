using System.Collections.Generic;
using Zeus.AddIns.Mailouts.ContentTypes;
using Zeus.AddIns.Mailouts.Services;
using Zeus.Admin;

namespace Zeus.AddIns.Mailouts.Admin
{
	public partial class CampaignRecipients : AdminPage
	{
		protected override void OnPreInit(System.EventArgs e)
		{
			MasterPageFile = "~/Admin/Popup.master";
			base.OnPreInit(e);
		}

		protected IEnumerable<IMailoutRecipient> GetRecipients()
		{
			return Zeus.Context.Current.Resolve<IMailoutService>().GetRecipients((Campaign) SelectedItem);
		}
	}
}
