using System;

namespace Bermedia.Gibbons.Web.UI.UserControls
{
	public partial class ProductRows : System.Web.UI.UserControl
	{
		public int GroupItemCount
		{
			set { lsvProducts.GroupItemCount = value; }
		}

		public object DataSource
		{
			set { lsvProducts.DataSource = value; }
		}
	}
}