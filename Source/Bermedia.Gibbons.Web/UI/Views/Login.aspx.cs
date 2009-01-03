using System;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class Login : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack && User.Identity.IsAuthenticated && Request.QueryString["ReturnUrl"] != null)
			{
				// check if user has access to the page they're trying to get to - if so, redirect them immediately
				string returnUrl = Request.QueryString["ReturnUrl"];
				if (returnUrl.Contains("?"))
					returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("?"));
				if (UrlAuthorizationModule.CheckUrlAccessForPrincipal(returnUrl, User, "GET"))
					Response.Redirect(Request.QueryString["ReturnUrl"]);
			}
		}

		protected void lgnLogin_LoggedIn(object sender, EventArgs e)
		{
			// Find user that just logged in.
			TextBox txtUserName = (TextBox) lgnLogin.FindControl("UserName");
			MembershipUser membershipUser = Membership.GetUser(txtUserName.Text);
			Session["CustomerID"] = membershipUser.ProviderUserKey;

			// Redirect.
			if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
				Response.Redirect(Request.QueryString["ReturnUrl"]);
		}
	}
}
