using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.Web.UI;
using Zeus.Web.UI.WebControls;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class NonFragranceBeautyCategory : ContentPage<Bermedia.Gibbons.Web.Items.NonFragranceBeautyCategory>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.CurrentItem.Image != null)
			{
				ItemDetailView itemDetailView = new ItemDetailView { PropertyName = "Image" };
				uscProductListing.ImageControl = itemDetailView;
			}
			uscProductListing.DataSource = this.CurrentItem.GetChildren<Items.StandardProduct>()
				.OrderBy(p => p.Title).OrderBy(p => p.Brand.Title);
		}
	}
}
