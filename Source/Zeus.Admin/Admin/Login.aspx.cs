using System;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using Zeus.Configuration;

namespace Zeus.Admin
{
	public partial class Login : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			ltlAdminName.Text = ((AdminSection) ConfigurationManager.GetSection("zeus/admin")).Name;
		}

		protected void lgnLogin_Authenticate(object sender, AuthenticateEventArgs e)
		{
			try
			{
				TextBox txtUserName = (TextBox) lgnLogin.FindControl("UserName");
				TextBox txtPassword = (TextBox) lgnLogin.FindControl("Password");
				if (FormsAuthentication.Authenticate(txtUserName.Text, txtPassword.Text))
				{
					e.Authenticated = true;
					FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, lgnLogin.RememberMeSet);
				}
				else if (System.Web.Security.Membership.ValidateUser(txtUserName.Text, txtPassword.Text))
				{
					e.Authenticated = true;
					FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, lgnLogin.RememberMeSet);
				}
			}
			catch (Exception ex)
			{
				Trace.Warn(ex.ToString());
				e.Authenticated = false;
			}
		}
	}
}
