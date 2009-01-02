using System;
using System.Linq;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class ViewOrder : SecurePage<Items.Order>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				lsvShoppingCartItems.DataSource = this.CurrentItem.GetChildren<Web.Items.OrderItem>();
				lsvShoppingCartItems.DataBind();
			}
		}
	}
}
