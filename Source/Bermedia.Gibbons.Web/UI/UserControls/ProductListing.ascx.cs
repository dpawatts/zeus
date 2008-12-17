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

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			DataSourceControl dataSourceControl = this.NamingContainer.FindControl(this.DataSourceID) as DataSourceControl;
			lsvProducts.DataSource = dataSourceControl;
			lsvProducts.DataBind();
		}
	}
}