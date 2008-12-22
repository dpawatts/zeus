using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bermedia.Gibbons.Web.UI.UserControls
{
	public partial class ProductListing : System.Web.UI.UserControl
	{
		public string DataSourceID
		{
			get { return ViewState["DataSourceID"] as string ?? string.Empty; }
			set { ViewState["DataSourceID"] = value; }
		}

		public string HeaderText
		{
			set
			{
				h1Header.InnerText = value;
				h1Header.Visible = !string.IsNullOrEmpty(value);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			DataSourceControl dataSourceControl = this.NamingContainer.FindControl(this.DataSourceID) as DataSourceControl;
			lsvProducts.DataSource = dataSourceControl;
			lsvProducts.DataBind();

			if (lsvProducts.Items.Count == 0)
				h1Header.Visible = false;
		}
	}
}