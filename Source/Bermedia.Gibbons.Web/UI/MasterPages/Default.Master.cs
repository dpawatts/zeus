using System;
using System.Linq;
using Zeus.Web.UI.WebControls;

namespace Bermedia.Gibbons.Web.UI.MasterPages
{
	public partial class Default : System.Web.UI.MasterPage
	{
		protected void zeusMenu_MenuItemCreating(object sender, MenuItemCreatingEventArgs e)
		{
			if ((e.CurrentItem is Items.BaseDepartment) && e.CurrentItem.GetChildren<Web.Items.BaseDepartment>().Any())
				e.Url = string.Empty;
		}
	}
}
