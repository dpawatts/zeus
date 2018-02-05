using System;
using System.Linq;
using System.Web.UI.WebControls;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.Admin;

namespace Zeus.AddIns.ECommerce.Admin.Plugins.ManageOrders
{
	public partial class Search : PreviewFrameAdminPage
	{
		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}

		private void DoSearch()
		{
            if (!string.IsNullOrEmpty(txtOrderNumber.Text))
            {
                //see if valid order number
                int OrderID = Convert.ToInt32(txtOrderNumber.Text);
                lsvOrders.DataSource = SelectedItem.GetChildren<Order>().Where(o => o.ID == OrderID).ToList();
            }
            else
            {
                lsvOrders.DataSource = SelectedItem.GetChildren<Order>().Where(o => 
                    (!string.IsNullOrEmpty(txtCustomerEmail.Text) && o.EmailAddress.Contains(txtCustomerEmail.Text)) ||
                    (!string.IsNullOrEmpty(txtCustomerFirstName.Text) && o.BillingAddress.FirstName.Contains(txtCustomerFirstName.Text)) ||
                    (!string.IsNullOrEmpty(txtCustomerLastName.Text) && o.BillingAddress.Surname.Contains(txtCustomerLastName.Text))).ToList();
            }
			lsvOrders.DataBind();
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(PreviewFrameAdminPage), "Zeus.Admin.Assets.Css.view.css");
			base.OnPreRender(e);
		}

		#endregion

        protected void btnSearch_Click(object sender, EventArgs e)
		{
            DoSearch();
		}        

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("admin.plugins.manage-orders.default.aspx?selected=" + Request.QueryString["selected"]);
        }

		protected void lsvOrders_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
		{
			dpgSearchResultsPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            DoSearch();
		}
	}
}