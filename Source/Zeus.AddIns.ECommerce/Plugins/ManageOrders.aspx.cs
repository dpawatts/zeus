using System;
using System.Linq;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web.UI;
using Isis.Web.Hosting;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.Admin;

[assembly: EmbeddedResourceFile("Zeus.AddIns.ECommerce.Plugins.ManageOrders.aspx", "Zeus.AddIns.ECommerce")]
namespace Zeus.AddIns.ECommerce.Plugins
{
	[ECommerceActionPluginAttribute("ManageOrders", "Manage Orders", 1, "Zeus.AddIns.ECommerce.Plugins.ManageOrders.aspx", "Zeus.AddIns.ECommerce.Icons.basket.png")]
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