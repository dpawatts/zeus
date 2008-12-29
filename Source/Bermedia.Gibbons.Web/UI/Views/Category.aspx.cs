using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class Category : ContentPage<Bermedia.Gibbons.Web.Items.BaseCategory>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.CurrentItem.Image != null)
			{
				uscProductListing1.ProductsPerRow = 2;
				uscProductListing1.DataSource = this.CurrentItem.GetChildren<Items.StandardProduct>().Take(4).OrderBy(p => p.Title).OrderBy(p => p.Brand.Title);
				uscProductListing2.DataSource = this.CurrentItem.GetChildren<Items.StandardProduct>().Skip(4).OrderBy(p => p.Title).OrderBy(p => p.Brand.Title);
			}
			else
			{
				uscProductListing1.DataSource = this.CurrentItem.GetChildren<Items.StandardProduct>().OrderBy(p => p.Title).OrderBy(p => p.Brand.Title);
				uscProductListing2.Visible = false;
			}
		}
	}
}
