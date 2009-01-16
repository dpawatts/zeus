using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class Search : ContentPage<Items.SearchPage>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			string searchText = Request.GetRequiredString("q").ToLower();
			rptSearchResults.DataSource = Zeus.Context.Current.Finder.Elements<Items.StandardProduct>()
				.OrderBy(p => p.Title)
				.ToList()
				.OrderBy(p => p.Brand.Title)
				.Where(p => p.Brand.Title.ToLower().Contains(searchText) || p.Title.ToLower().Contains(searchText) || p.Description.ToLower().Contains(searchText))
				.GroupBy(p => p.Department)
				.OrderBy(d => d.Key.Title);
			rptSearchResults.DataBind();
		}
	}
}
