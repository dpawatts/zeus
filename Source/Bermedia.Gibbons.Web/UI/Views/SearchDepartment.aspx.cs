﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class SearchDepartment : ContentPage<Items.BaseDepartment>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			string searchText = Request.GetRequiredString("q").ToLower();
			uscProductListing.DataSource = Zeus.Find.EnumerateAccessibleChildren(this.CurrentItem).OfType<Web.Items.StandardProduct>()
				.Where(p => p.Brand.Title.ToLower().Contains(searchText) || p.Title.ToLower().Contains(searchText) || p.Description.ToLower().Contains(searchText))
				.OrderBy(p => p.Title).OrderBy(p => p.Brand.Title);
		}
	}
}