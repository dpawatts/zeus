using System;
using System.Linq;
using Zeus.Web.UI;
using System.Web.UI.WebControls;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class ManageAddressBook : SecurePage<Items.MyAccount>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				lsvShippingAddresses.DataSource = this.Customer.ShippingAddresses;
				lsvShippingAddresses.DataBind();

				lsvBillingAddresses.DataSource = this.Customer.BillingAddresses;
				lsvBillingAddresses.DataBind();
			}
		}

		protected void lsvShippingAddresses_ItemDeleting(object sender, ListViewDeleteEventArgs e)
		{
			Web.Items.Customer customer = this.Customer;
			customer.ShippingAddresses.Remove(customer.ShippingAddresses[e.ItemIndex]);
			Zeus.Context.Persister.Save(customer);

			lsvShippingAddresses.DataSource = this.Customer.ShippingAddresses;
			lsvShippingAddresses.DataBind();
		}

		protected void lsvBillingAddresses_ItemDeleting(object sender, ListViewDeleteEventArgs e)
		{
			Web.Items.Customer customer = this.Customer;
			customer.BillingAddresses.Remove(customer.BillingAddresses[e.ItemIndex]);
			Zeus.Context.Persister.Save(customer);

			lsvBillingAddresses.DataSource = this.Customer.BillingAddresses;
			lsvBillingAddresses.DataBind();
		}
	}
}
