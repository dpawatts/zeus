using System;
using System.Linq;
using System.Web.UI.WebControls;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.Admin;

namespace Zeus.AddIns.ECommerce.Admin.Plugins.ManageOrders
{
	public partial class Default : PreviewFrameAdminPage
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
			lsvOrders.DataSource = SelectedItem.GetChildren<Order>().Where(o => o.Status == OrderStatus.Paid).OrderByDescending(o => o.ID).ToList();
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

        protected void btnSeeAll_Click(object sender, EventArgs e)
		{
            lsvOrders.DataSource = SelectedItem.GetChildren<Order>().Where(o => o.Status != OrderStatus.Unpaid).OrderByDescending(o => o.ID).ToList();
            lsvOrders.DataBind();
		}

        protected void btnSearch_Click(object sender, EventArgs e)
		{
			
		}

		protected void lsvOrders_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
		{
			dpgSearchResultsPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
			ReBind();
		}
	}
}