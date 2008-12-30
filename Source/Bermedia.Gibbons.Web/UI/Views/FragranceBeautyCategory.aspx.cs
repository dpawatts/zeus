using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class FragranceBeautyCategory : ContentPage<Bermedia.Gibbons.Web.Items.FragranceBeautyCategory>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			uscProductListing.DataSource = Zeus.Find.EnumerateChildren(this.CurrentItem).OfType<Web.Items.FragranceBeautyProduct>()
				.OrderBy(p => p.Title)
				.OrderBy(p => p.Brand.Title)
				.OrderBy(p => (p.Strength != null) ? p.Strength.SortOrder : 0)
				.Cast<Web.Items.StandardProduct>();
		}
	}
}
