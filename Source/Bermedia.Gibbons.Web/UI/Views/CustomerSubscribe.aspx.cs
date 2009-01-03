using System;
using System.Linq;
using System.Net.Mail;
using Zeus.Admin;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class CustomerSubscribe : SecurePage<Web.Items.MyAccount>
	{
		protected void btnSubscribe_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid)
				return;

			Web.Items.NewsletterSubscription ns = new Web.Items.NewsletterSubscription();
			ns.EmailAddress = this.Customer.Email;
			ns.FirstName = this.Customer.FirstName;
			ns.LastName = this.Customer.LastName;
			ns.AddTo(Zeus.Find.RootItem.GetChild("Newsletter Subscriptions"));
			Zeus.Context.Persister.Save(ns);

			Web.Items.Customer customer = this.Customer;
			customer.NewsletterSubscription = ns;
			Zeus.Context.Persister.Save(customer);

			SendNewsletterSubscriptionConfirmation(this.Customer.Email);

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
