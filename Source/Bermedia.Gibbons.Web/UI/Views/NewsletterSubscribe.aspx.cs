using System;
using System.Linq;
using System.Net.Mail;
using Zeus.Admin;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class NewsletterSubscribe : System.Web.UI.Page
	{
		protected void csvEmailAddress_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs e)
		{
			bool isValid = !Zeus.Context.Current.Finder.Elements<Web.Items.Customer>().ToList().Any(c => string.Equals(c.Email, txtEmailAddress.Text, StringComparison.CurrentCultureIgnoreCase));
			if (isValid)
				isValid = !Zeus.Context.Current.Finder.Elements<Web.Items.NewsletterSubscription>().ToList().Any(ns => string.Equals(ns.EmailAddress, txtEmailAddress.Text, StringComparison.CurrentCultureIgnoreCase));
			e.IsValid = isValid;
		}

		protected void btnSubscribe_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid)
				return;

			Web.Items.NewsletterSubscription ns = new Web.Items.NewsletterSubscription();
			ns.EmailAddress = txtEmailAddress.Text;
			ns.FirstName = txtFirstName.Text;
			ns.LastName = txtLastName.Text;
			ns.AddTo(Zeus.Find.RootItem.GetChild("Newsletter Subscriptions"));
			Zeus.Context.Persister.Save(ns);

			try
			{
				SendNewsletterSubscriptionConfirmation(txtEmailAddress.Text);
			}
			catch (SmtpException)
			{
				csvEmailAddress.ErrorMessage = "Could not send confirmation e-mail";
				csvEmailAddress.IsValid = false;
				return;
			}

			plcForm.Visible = false;
			plcConfirmation.Visible = true;
		}

		private static void SendNewsletterSubscriptionConfirmation(string emailAddress)
		{
			string messageText = string.Format(@"Thanks for giving gibbons.bm a try!

This email has been sent to confirm that {0} has been added to the list of customers that receive email from gibbons.bm.

Soon, you'll be among the first to know about sales, exciting offers, new products and special seasonal events.

Thanks for your interest and thanks for shopping at gibbons.bm.
http://www.gibbons.bm", emailAddress);

			SmtpClient smtpClient = new SmtpClient();
			smtpClient.Send("newsletters@gibbons.bm", emailAddress, "Gibbons.bm Newsletter Subscription Confirmation", messageText);
		}
	}
}
