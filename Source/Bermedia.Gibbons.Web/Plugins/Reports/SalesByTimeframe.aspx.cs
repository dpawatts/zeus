using System;
using System.Linq;
using Zeus.Admin;
using Zeus.Linq;
using System.Collections.Generic;
using Bermedia.Gibbons.Web.Items;
using Zeus;

namespace Bermedia.Gibbons.Web.Plugins.Reports
{
	public partial class SalesByTimeframe : AdminPage
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			BindSalesTable(uscDateRange.StartDate, uscDateRange.EndDate);
		}

		private void BindSalesTable(DateTime startDate, DateTime endDate)
		{
			lsvSales.DataSource = SelectSales_ByTimeframe(startDate, endDate);
			lsvSales.DataBind();
		}

		private static IEnumerable<SalesData> SelectSales_ByTimeframe(DateTime startDate, DateTime endDate)
		{
			var orders = Zeus.Context.Current.Finder.Elements<Order>().ToArray();

			var salesData = new SalesData[5];
			salesData[0] = new SalesData
			{
				Description = string.Format("Total {0:dd MMM yyyy} to {1:dd MMM yyyy}", startDate, endDate),
				Total = orders.Where(o => o.DatePlaced.Date >= startDate && o.DatePlaced.Date <= endDate).Sum(o => o.TotalPrice)
			};
			salesData[1] = new SalesData
			{
				Description = string.Format("Total Today ({0:dd MMM yyyy})", DateTime.Today),
				Total = orders.Where(o => o.DatePlaced.Date == DateTime.Today).Sum(o => o.TotalPrice)
			};
			salesData[2] = new SalesData
			{
				Description = "Total Yesterday",
				Total = orders.Where(o => o.DatePlaced.Date == DateTime.Today.AddDays(-1)).Sum(o => o.TotalPrice)
			};
			salesData[3] = new SalesData
			{
				Description = "Total Last 7 Days",
				Total = orders.Where(o => o.DatePlaced.Date >= DateTime.Today.AddDays(7) && o.DatePlaced.Date <= DateTime.Today).Sum(o => o.TotalPrice)
			};
			salesData[4] = new SalesData
			{
				Description = string.Format("Total To Date This Month ({0})", DateTime.Today.ToString("MMMM")),
				Total = orders.Where(o => o.DatePlaced.Date.Year == DateTime.Today.Year && o.DatePlaced.Date.Month == DateTime.Today.Month).Sum(o => o.TotalPrice)
			};

			return salesData;
		}

		public class SalesData
		{
			public string Description
			{
				get;
				set;
			}

			public decimal Total
			{
				get;
				set;
			}
		}
	}
}
