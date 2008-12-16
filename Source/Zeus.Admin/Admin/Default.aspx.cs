using System;
using Zeus.Configuration;
using System.Configuration;

namespace Zeus.Admin
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			imgLogo.Visible = !((AdminSection) ConfigurationManager.GetSection("zeus/admin")).HideBranding;
			ltlAdminName.Text = ((AdminSection) ConfigurationManager.GetSection("zeus/admin")).Name;
		}
	}
}
