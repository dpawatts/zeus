using System;
using System.Linq;
using Quiksoft.EasyMail.SMTP;
using Isis.Web;
using Zeus.Admin;
using Bermedia.Gibbons.Web.Items;

namespace Bermedia.Gibbons.Web.Plugins.Newsletters
{
	public partial class SendTest : AdminPage
	{
		protected Newsletter SelectedNewsletter
		{
			get { return this.SelectedItem as Newsletter; }
		}

		protected void btnSend_Click(object sender, EventArgs e)
		{
			MailoutManager.SetLicenseKey();

			// Send test email.
			EmailMessage email = MailoutManager.GetEmailMessage(this.SelectedNewsletter);
			email.Recipients.Add(txtEmailAddress.Text);

			MailoutManager.Send(smtp =>
				smtp.Send(email)
			);

			plcForm.Visible = false;
			plcConfirmation.Visible = true;
		}
	}
}