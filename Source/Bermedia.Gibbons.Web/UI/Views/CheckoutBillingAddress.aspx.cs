using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis;
using Bermedia.Gibbons.Web.Items;
using SoundInTheory.DynamicImage;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class CheckoutBillingAddress : SecurePage<Web.Items.CheckoutBillingAddress>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			lsvAddressBook.DataBind();
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			if (!IsValid)
				return;

			// Save address
			Address address = new Address();
			address.AddressLine1 = txtAddress1.Text;
			address.AddressLine2 = txtAddress2.Text;
			address.City = txtCity.Text;
			address.ParishState = txtParishState.Text;
			address.Country = Zeus.Context.Persister.Get<Country>(Convert.ToInt32(ddlCountry.SelectedValue));
			address.PhoneNumber = txtPhone.Text;
			this.Customer.BillingAddresses.Add(address);
			Zeus.Context.Persister.Save(address);

			SaveAddressAndRedirect(address.ID);
		}

		protected void btnUseAddress_Click(object sender, EventArgs e)
		{
			SaveAddressAndRedirect(Convert.ToInt32(((DynamicImageButton) sender).CommandArgument));
		}

		private void SaveAddressAndRedirect(int addressID)
		{
			this.ShoppingCart.BillingAddress = Zeus.Context.Persister.Get<Address>(addressID);
			Zeus.Context.Persister.Save(this.ShoppingCart);

			if (Request.QueryString["return"] != null)
				Response.Redirect("~/checkout-summary.aspx");
			else
				Response.Redirect("~/checkout-summary.aspx");
		}
	}
}
