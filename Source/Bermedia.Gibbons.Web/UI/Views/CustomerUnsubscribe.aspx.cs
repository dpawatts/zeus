using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class CustomerUnsubscribe : SecurePage<Web.Items.MyAccount>
	{
		protected void btnUnsubscribe_Click(object sender, EventArgs e)
		{
			Zeus.Context.Persister.Delete(this.Customer.NewsletterSubscription);

			plcForm.Visible = false;
			plcConfirmation.Visible = true;
		}
	}
}
