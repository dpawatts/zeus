using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class Product : ContentPage<Bermedia.Gibbons.Web.Items.StandardProduct>
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void btnAddToCart_Click(object sender, EventArgs e)
		{
			string url = string.Format("~/shopping-cart.aspx?add={0}&quantity={1}", this.CurrentItem.ID, txtQuantity.Text);
			if (!string.IsNullOrEmpty(ddlSizes.SelectedValue))
				url += string.Format("&size={0}", ddlSizes.SelectedValue);
			if (!string.IsNullOrEmpty(ddlColours.SelectedValue))
				url += string.Format("&colour={0}", ddlColours.SelectedValue);
			Response.Redirect(url);
		}
	}
}
