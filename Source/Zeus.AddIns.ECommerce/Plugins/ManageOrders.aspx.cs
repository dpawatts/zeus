using System;
using System.Linq;
using System.Web.UI.WebControls;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.Admin;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;

namespace Zeus.AddIns.ECommerce.Plugins
{
	public partial class ManageOrders : PreviewFrameAdminPage
	{
		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			if (!IsPostBack)
				ReBind();

			base.OnLoad(e);
		}

		private void ReBind()
		{
			lsvOrders.DataSource = SelectedItem.GetChildren<Order>().OrderByDescending(o => o.Created);
			lsvOrders.DataBind();
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(PreviewFrameAdminPage), "Zeus.Admin.Assets.Css.view.css");
			base.OnPreRender(e);
		}

		#endregion

		protected void btnProcess_Command(object sender, CommandEventArgs e)
		{
			int orderID = Convert.ToInt32(e.CommandArgument);

			Order order = Engine.Persister.Get<Order>(orderID);
			order.Status = OrderStatus.Processed;
			Engine.Persister.Save(order);

			ReBind();
		}
	}
}