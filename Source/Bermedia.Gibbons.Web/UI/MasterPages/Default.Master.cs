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
			if ((e.CurrentItem is Items.BaseDepartment))
			{
				Items.BaseDepartment department = (Items.BaseDepartment) e.CurrentItem;
				if (!department.ShowPageIfChildDepartmentsExist && e.CurrentItem.GetChildren<Items.BaseDepartment>().Any())
					e.Url = string.Empty;
			}
		}

		protected void btnLogout_Click(object sender, EventArgs e)
		{
			FormsAuthentication.SignOut();
			Response.Redirect("~/");
		}
	}
}
