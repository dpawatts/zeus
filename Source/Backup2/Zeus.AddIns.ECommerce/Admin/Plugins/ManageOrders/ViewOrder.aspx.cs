using System;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.Admin;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;

namespace Zeus.AddIns.ECommerce.Plugins
{
	public partial class ViewOrder : PreviewFrameAdminPage
	{
        protected Order SelectedOrder
		{
			get { return (Order) SelectedItem; }
		}

		#region Methods

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(PreviewFrameAdminPage), "Zeus.Admin.Assets.Css.view.css");
			base.OnPreRender(e);
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SelectedOrder.Status != OrderStatus.Paid)
                btnProcess.Visible = false;

            if (SelectedOrder.Status != OrderStatus.Paid)
                btnCancel.Visible = false;
        }

		#endregion

		protected void btnProcess_Click(object sender, EventArgs e)
		{
			Order order = SelectedOrder;
			order.Status = OrderStatus.Processed;
			Engine.Persister.Save(order);

            Response.Redirect("admin.plugins.manage-orders.view-order.aspx?selected=" + order.Path);
		}

		protected void btnBack_Click(object sender, EventArgs e)
		{
            Order order = SelectedOrder;
            
            Response.Redirect("admin.plugins.manage-orders.default.aspx?selected=" + order.Parent.Path);
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Order order = SelectedOrder;
			order.Status = OrderStatus.Cancelled;
			Engine.Persister.Save(order);

            Response.Redirect("admin.plugins.manage-orders.view-order.aspx?selected=" + order.Path);
		}
	}
}