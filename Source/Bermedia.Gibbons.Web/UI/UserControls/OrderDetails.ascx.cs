using System;
using System.Web.UI;

namespace Bermedia.Gibbons.Web.UI.UserControls
{
	public partial class OrderDetails : System.Web.UI.UserControl
	{
		public OrderDetailsViewMode ViewMode
		{
			get;
			set;
		}

		public new SecurePage Page
		{
			get { return (SecurePage) base.Page; }
		}

		public object DataSource
		{
			set
			{
				fmvOrderDetails.DataSource = value;
				fmvOrderDetails.DataBind();
			}
		}
	}

	public enum OrderDetailsViewMode
	{
		CheckoutSummary,
		Other
	}
}