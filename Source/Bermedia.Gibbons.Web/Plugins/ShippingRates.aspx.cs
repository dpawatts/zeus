using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bermedia.Gibbons.Web.Items;
using Zeus.Admin;

namespace Bermedia.Gibbons.Web.Plugins
{
	[Zeus.Admin.ToolbarPlugin("~/Plugins/ShippingRates.aspx", AdminTargetFrame.Preview, "~/Assets/Images/Icons/package_go.png", 100, ToolTip = "International Shipping Rates")]
	public partial class ShippingRates : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			IEnumerable<InternationalShippingDeliveryType> deliveryTypes = Zeus.Context.Current.Finder.ToArray().OfType<InternationalShippingDeliveryType>();

			TableRow tr = new TableRow();
			tr.CssClass = "titles";
			tr.Cells.Add(new TableHeaderCell());
			tr.Cells.AddRange(deliveryTypes.Select(dt => new TableHeaderCell { Text = dt.Title }).ToArray());
			tblShippingRates.Rows.Add(tr);

			IEnumerable<InternationalShippingPriceRange> priceRanges = Zeus.Context.Current.Finder.ToArray().OfType<InternationalShippingPriceRange>();
			IEnumerable<InternationalShippingRate> rates = Zeus.Context.Current.Finder.ToArray().OfType<InternationalShippingRate>();
			foreach (InternationalShippingPriceRange priceRange in priceRanges)
			{
				TableRow tableRow = new TableRow();
				tableRow.ID = "PriceRange_" + priceRange.ID.ToString();
				tableRow.Cells.Add(new TableCell { Text = priceRange.Title });
				foreach (InternationalShippingDeliveryType deliveryType in deliveryTypes)
					tableRow.Cells.Add(GetTextBoxCell(
						priceRange.ID,
						deliveryType.ID,
						rates.SingleOrDefault(r => r.PriceRange == priceRange && r.DeliveryType == deliveryType)));
				tblShippingRates.Rows.Add(tableRow);
			}
		}

		private TableCell GetTextBoxCell(int priceRangeID, int deliveryTypeID, InternationalShippingRate price)
		{
			TableCell cell = new TableCell();
			cell.ID = priceRangeID + "_DeliveryType_" + deliveryTypeID;
			TextBox tb = new TextBox();
			if (price != null)
				tb.Text = price.Price.ToString("F2");
			cell.Controls.Add(tb);
			return cell;
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			IEnumerable<InternationalShippingRate> rates = Zeus.Context.Current.Finder.ToArray().OfType<InternationalShippingRate>();
			for (int i = 1; i < tblShippingRates.Rows.Count; i++)
			{
				TableRow tr = tblShippingRates.Rows[i];
				int priceRangeID = Convert.ToInt32(tr.ID.Split(new char[] { '_' })[1]);

				for (int j = 1; j < tr.Cells.Count; j++)
				{
					TableCell tc = tr.Cells[j];
					int deliveryTypeID = Convert.ToInt32(tc.ID.Split(new char[] { '_' })[2]);

					InternationalShippingRate price = rates.SingleOrDefault(r => r.PriceRange.ID == priceRangeID && r.DeliveryType.ID == deliveryTypeID);
					if (price == null)
						price = new InternationalShippingRate { PriceRange = (InternationalShippingPriceRange) Zeus.Context.Persister.Get(priceRangeID), DeliveryType = (InternationalShippingDeliveryType) Zeus.Context.Persister.Get(deliveryTypeID) };

					TextBox tb = (TextBox) tc.Controls[0];
					decimal newPrice = (!string.IsNullOrEmpty(tb.Text)) ? Convert.ToDecimal(tb.Text) : 0;
					price.Price = newPrice;
					Zeus.Context.Persister.Save(price);
				}
			}
		}
	}
}
