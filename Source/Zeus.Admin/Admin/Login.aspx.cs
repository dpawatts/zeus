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
				if (FormsAuthentication.Authenticate(lgnLogin.UserName, lgnLogin.Password))
				{
					e.Authenticated = true;
					FormsAuthentication.RedirectFromLoginPage(lgnLogin.UserName, lgnLogin.RememberMeSet);
				}
				else if (System.Web.Security.Membership.ValidateUser(lgnLogin.UserName, lgnLogin.Password))
				{
					e.Authenticated = true;
					FormsAuthentication.RedirectFromLoginPage(lgnLogin.UserName, lgnLogin.RememberMeSet);
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
