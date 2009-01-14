using System;
using Isis.ExtensionMethods.Web;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class EditAddress : SecurePage<Items.MyAccount>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Web.Items.Address address = Zeus.Context.Persister.Get<Web.Items.Address>(Request.GetRequiredInt("id"));
				txtAddress1.Text = address.AddressLine1;
				txtAddress2.Text = address.AddressLine2;
				txtCity.Text = address.City;
				txtParishState.Text = address.ParishState;
				txtZip.Text = address.Zip;
				txtPhone.Text = address.PhoneNumber;
				ddlCountry.SelectedValue = address.Country.ID.ToString();
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			if (!IsValid)
				return;

			// Save address
			Web.Items.Address address = Zeus.Context.Persister.Get<Web.Items.Address>(Request.GetRequiredInt("id"));
			address.AddressLine1 = txtAddress1.Text;
			address.AddressLine2 = txtAddress2.Text;
			address.City = txtCity.Text;
			address.ParishState = txtParishState.Text;
			address.Country = Zeus.Context.Persister.Get<Web.Items.Country>(Convert.ToInt32(ddlCountry.SelectedValue));
			address.PhoneNumber = txtPhone.Text;
			address.Zip = txtZip.Text;
			Zeus.Context.Persister.Save(address);

			Response.Redirect(new Zeus.Web.Url(this.CurrentItem.Url).AppendSegment("manage-address-book").ToString());
		}
	}
}
