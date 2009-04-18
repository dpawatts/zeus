using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis;
using Bermedia.Gibbons.Web.Items;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class CheckoutDeliveryMethod : SecurePage<Web.Items.CheckoutDeliveryMethod>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				rblDeliveryMethod.DataSource = Zeus.Find.EnumerateChildren(Zeus.Find.RootItem).OfType<DeliveryType>();
				rblDeliveryMethod.DataBind();
			}

			IEnumerable<InternationalShippingDeliveryType> deliveryTypes = Zeus.Context.Current.Finder.OfType<InternationalShippingDeliveryType>();

			TableRow tr = new TableRow();
			tr.Cells.Add(new TableHeaderCell { Width = Unit.Pixel(150) });
			tr.Cells.AddRange(deliveryTypes.Select(dt => new TableHeaderCell { Text = "<strong>" + dt.TitleWithLineBreak + "</strong>" }).ToArray());
			tblInternationalShippingRates.Rows.Add(tr);

			IEnumerable<InternationalShippingPriceRange> priceRanges = Zeus.Context.Current.Finder.OfType<InternationalShippingPriceRange>();
			IEnumerable<InternationalShippingRate> rates = Zeus.Context.Current.Finder.OfType<InternationalShippingRate>();
			foreach (InternationalShippingPriceRange priceRange in priceRanges)
			{
				TableRow tableRow = new TableRow();
				tableRow.Cells.Add(new TableCell { Text = "<p><strong>" + priceRange.Title + "</strong></p>" });
				foreach (InternationalShippingDeliveryType deliveryType in deliveryTypes)
					tableRow.Cells.Add(new TableCell { Text = rates.Single(r => r.PriceRange == priceRange && r.DeliveryType == deliveryType).Price.ToString("C2") });
				tblInternationalShippingRates.Rows.Add(tableRow);
			}
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			if (!IsValid)
				return;

			this.ShoppingCart.DeliveryType = Zeus.Context.Persister.Get<Web.Items.BaseDeliveryType>(Convert.ToInt32(rblDeliveryMethod.SelectedValue));
			Zeus.Context.Persister.Save(this.ShoppingCart);

			if (this.ShoppingCart.DeliveryType.RequiresShippingAddress)
				Response.Redirect("~/checkout-shipping-address.aspx");
			else
				Response.Redirect("~/checkout-payment-details.aspx");
		}
	}
}
