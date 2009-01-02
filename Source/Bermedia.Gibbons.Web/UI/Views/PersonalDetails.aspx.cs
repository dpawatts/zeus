using System;
using System.Linq;
using Zeus.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class PersonalDetails : SecurePage<Items.MyAccount>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				txtNewFirstName.Text = this.Customer.FirstName;
				txtNewLastName.Text = this.Customer.LastName;
				txtNewEmail.Text = this.Customer.Email;
			}
		}

		protected void btnSaveChanges_Click(object sender, EventArgs e)
		{
			if (!this.IsValid)
				return;

			Web.Items.Customer customer = this.Customer;
			customer.FirstName = txtNewFirstName.Text;
			customer.LastName = txtNewLastName.Text;
			customer.Email = txtNewEmail.Text;

			if (!string.IsNullOrEmpty(txtNewPassword.Text))
				customer.Password = txtNewPassword.Text;

			Zeus.Context.Persister.Save(customer);

			FormsAuthentication.SignOut();
			Response.Redirect("~/login.aspx?ReturnUrl=" + Server.UrlEncode(Request.RawUrl));
		}

		protected void csvNewEmail_ServerValidate(object source, ServerValidateEventArgs e)
		{
			MembershipUser membershipUser = Membership.GetUser(e.Value);
			e.IsValid = !(membershipUser != null && membershipUser.Email != this.Customer.Email);
		}

		protected void csvOldPassword_ServerValidate(object source, ServerValidateEventArgs e)
		{
			e.IsValid = (e.Value == this.Customer.Password);
		}
	}
}
