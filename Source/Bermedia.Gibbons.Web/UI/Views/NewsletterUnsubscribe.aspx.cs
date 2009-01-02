using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class NewsletterUnsubscribe : System.Web.UI.Page
	{
		protected void csvEmailAddress_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs e)
		{
			e.IsValid = Zeus.Context.Current.Finder.Elements<Web.Items.NewsletterSubscription>()
				.ToList().Any(ns => string.Equals(ns.EmailAddress, txtEmailAddress.Text, StringComparison.CurrentCultureIgnoreCase));
		}

		protected void btnUnsubscribe_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid)
				return;

			Web.Items.NewsletterSubscription subscription = Zeus.Context.Current.Finder.Elements<Web.Items.NewsletterSubscription>().ToList()
				.Single(ns => string.Equals(ns.EmailAddress, txtEmailAddress.Text, StringComparison.CurrentCultureIgnoreCase));
			Zeus.Context.Persister.Delete(subscription);

			plcForm.Visible = false;
			plcConfirmation.Visible = true;
		}
	}
}
