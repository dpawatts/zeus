using System;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class Register : System.Web.UI.Page
	{
		protected void cuwRegister_CreatedUser(object sender, EventArgs e)
		{
			if (!IsValid)
				return;

			// find user that was just created
			TextBox txtUserName = (TextBox) cusStep1.ContentTemplateContainer.FindControl("UserName");
			MembershipUser membershipUser = Membership.GetUser(txtUserName.Text);
			Web.Items.Customer customer = Zeus.Context.Persister.Get<Web.Items.Customer>((int) membershipUser.ProviderUserKey);

			// set copy e-mail address from username into e-mail field
			customer.Email = membershipUser.UserName;

			Roles.AddUserToRole(membershipUser.UserName, "Customer");

			// create a new customer record for them, with associated billing address
			TextBox txtFirstName = (TextBox) cusStep1.ContentTemplateContainer.FindControl("txtFirstName");
			TextBox txtLastName = (TextBox) cusStep1.ContentTemplateContainer.FindControl("txtLastName");
			CheckBox chkReceiveOffers = (CheckBox) cusStep1.ContentTemplateContainer.FindControl("chkReceiveOffers");

			customer.FirstName = txtFirstName.Text;
			customer.LastName = txtLastName.Text;

			if (chkReceiveOffers.Checked)
			{
				Web.Items.NewsletterSubscription subscription = new Web.Items.NewsletterSubscription();
				subscription.FirstName = customer.FirstName;
				subscription.LastName = customer.LastName;
				subscription.EmailAddress = customer.Email;
				Zeus.Context.Persister.Save(subscription);

				customer.NewsletterSubscription = subscription;
			}

			Zeus.Context.Persister.Save(customer);

			// Attempt login (we can't do this earlier since the user didn't have the role attached).
			TextBox txtPassword = (TextBox) cusStep1.ContentTemplateContainer.FindControl("Password");
			if (Membership.ValidateUser(membershipUser.UserName, txtPassword.Text))
				FormsAuthentication.SetAuthCookie(membershipUser.UserName, false);
		}

		protected void csvTerms_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs e)
		{
			CheckBox chkAgree = (CheckBox) cwsFinish.CustomNavigationTemplateContainer.FindControl("chkAgree");
			e.IsValid = chkAgree.Checked;
		}

		protected void cuwRegister_ContinueButtonClick(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
				Response.Redirect(Request.QueryString["ReturnUrl"]);
			else
				Response.Redirect("~/default.aspx");
		}
	}
}
