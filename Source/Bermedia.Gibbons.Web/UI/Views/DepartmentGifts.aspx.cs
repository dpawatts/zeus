using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.Web;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class DepartmentGifts : ContentPage<Bermedia.Gibbons.Web.Items.BaseDepartment>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			uscProductListing.DataSource = Zeus.Find.EnumerateAccessibleChildren(this.CurrentItem)
				.OfType<Web.Items.StandardProduct>()
				.Where(p => p.RegularPrice <= Request.GetRequiredInt("PriceLimit") && p.GiftItem);
		}
	}
}
