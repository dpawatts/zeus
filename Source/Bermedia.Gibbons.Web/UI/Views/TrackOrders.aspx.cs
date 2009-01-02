using System;
using System.Linq;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class TrackOrders : SecurePage<Items.MyAccount>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			lsvOrders.DataSource = this.Customer.Orders.OrderByDescending(o => o.DatePlaced);
			lsvOrders.DataBind();
		}
	}
}
