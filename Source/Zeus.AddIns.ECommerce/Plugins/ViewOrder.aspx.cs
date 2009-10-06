using System;
using Isis.ExtensionMethods.Web.UI;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.Admin;

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

		#endregion

		protected void btnProcess_Click(object sender, EventArgs e)
		{
			Order order = SelectedOrder;
			order.Status = OrderStatus.Processed;
			Engine.Persister.Save(order);

			Response.Redirect("manageorders.aspx?selected=" + order.Parent.Path);
		}
	}
}