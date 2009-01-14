using System;
using System.Linq;
using Zeus.Web.UI.WebControls;
using System.Web.Security;

namespace Bermedia.Gibbons.Web.UI.MasterPages
{
	public partial class Default : System.Web.UI.MasterPage
	{
		protected void zeusMenu_MenuItemCreating(object sender, MenuItemCreatingEventArgs e)
		{
			if ((e.CurrentItem is Items.BaseDepartment) && e.CurrentItem.GetChildren<Web.Items.BaseDepartment>().Any())
				e.Url = string.Empty;
		}

		protected void btnLogout_Click(object sender, EventArgs e)
		{
			FormsAuthentication.SignOut();
			Response.Redirect("~/");
		}
	}
}
