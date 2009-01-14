using System;
using System.Linq;
using Zeus.Admin;
using Zeus.Linq;
using System.Collections.Generic;
using Bermedia.Gibbons.Web.Items;
using Zeus;

namespace Bermedia.Gibbons.Web.Plugins.Reports
{
	public partial class SalesByCategory : AdminPage
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			BindSalesTable(uscDateRange.StartDate, uscDateRange.EndDate);
		}

		private void BindSalesTable(DateTime startDate, DateTime endDate)
		{
			lsvSales.DataSource = SelectSales_ByCategory(startDate, endDate);
			lsvSales.DataBind();
		}

		private static IEnumerable<SalesData> SelectSales_ByCategory(DateTime startDate, DateTime endDate)
		{
			var orderItemsGroupedByCategory = Zeus.Context.Current.Finder.Elements<Order>().ToArray().Where(o => o.DatePlaced.Date >= startDate && o.DatePlaced.Date <= endDate)
				.SelectMany(o => o.Children.OfType<OrderItem>()).GroupBy(oi => (BaseCategory) oi.Product.Parent);

			var salesData = orderItemsGroupedByCategory.Select(oi => new SalesData { Category = oi.Key.HierarchicalTitle, UnitsSold = oi.Count() });

			return salesData;
		}

		public class SalesData
		{
			public string Category
			{
				get;
				set;
			}

			public int UnitsSold
			{
				get;
				set;
			}
		}
	}
}
