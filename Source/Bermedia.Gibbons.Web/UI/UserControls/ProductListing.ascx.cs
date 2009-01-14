using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web;
using Zeus.Web.UI.WebControls;
using Zeus.Web;

namespace Bermedia.Gibbons.Web.UI.UserControls
{
	public partial class ProductListing : System.Web.UI.UserControl
	{
		public IEnumerable<Items.StandardProduct> DataSource
		{
			get;
			set;
		}

		public string HeaderText
		{
			set
			{
				h1Header.InnerText = value;
				h1Header.Visible = !string.IsNullOrEmpty(value);
			}
		}

		public int PageSize
		{
			get { return (int) (ViewState["PageSize"] ?? 8); }
			set { ViewState["PageSize"] = value; }
		}

		public Control ImageControl
		{
			get;
			set;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (this.ImageControl != null)
				pnlCategoryImage.Controls.Add(this.ImageControl);
			else
				pnlCategoryImage.Visible = false;

			if (this.DataSource != null)
			{
				// First get to the right page of products.
				const int PageSize = 8;
				int currentPage = 1;
				if (Request.GetOptionalInt("p") != null)
					currentPage = Request.GetRequiredInt("p");
				IEnumerable<Web.Items.StandardProduct> products = this.DataSource.Skip((currentPage - 1) * PageSize).Take(PageSize);

				// If ImageControl is not null, we need to split the first row into two rows.
				if (this.ImageControl != null)
				{
					AddSection(products.Take(PageSize / 2), uscProductRows1, 2);
					if (products.Skip(PageSize / 2).Take(PageSize / 2).Any())
						AddSection(products.Skip(PageSize / 2).Take(PageSize / 2), uscProductRows2, 4);
				}
				else
				{
					AddSection(products, uscProductRows1, 4);
				}

				if (!products.Any())
					h1Header.Visible = false;

				int totalPageCount = (int) Math.Ceiling((double) this.DataSource.Count() / PageSize);

				string pageLinks = string.Empty;
				for (int i = 1; i <= totalPageCount; i++)
				{
					if (i == currentPage)
						pageLinks += i.ToString();
					else
						pageLinks += string.Format("<a href=\"{0}\">{1}</a>", new Url(Request.RawUrl).SetQueryParameter("p", i).ToString(), i);
					if (i < totalPageCount)
						pageLinks += " | ";
				}
				ltlPageLinks.Text = pageLinks;
			}
			else
			{
				h1Header.Visible = false;
			}
		}

		private void AddSection(IEnumerable<Web.Items.StandardProduct> products, UserControls.ProductRows uscProductRows, int productsPerRow)
		{
			List<ProductGroup> groups = new List<ProductGroup>();
			AddRows(productsPerRow, products, groups);

			uscProductRows.GroupItemCount = productsPerRow;
			uscProductRows.DataSource = groups;
			uscProductRows.DataBind();
		}

		private int AddRows(int productsPerRow, IEnumerable<Web.Items.StandardProduct> products, List<ProductGroup> groups)
		{
			int addedRows = 0;
			for (int i = 0, length = products.Count(); i < length; i += productsPerRow)
			{
				ProductGroup group = new ProductGroup();
				for (int j = 0; j < productsPerRow; j++)
					if (AddProduct(products, i + j, group))
						++addedRows;
				groups.Add(group);
			}
			return addedRows;
		}

		private bool AddProduct(IEnumerable<Web.Items.StandardProduct> products, int i, ProductGroup group)
		{
			Items.StandardProduct product = products.ElementAtOrDefault(i);
			if (product != null)
			{
				group.Products.Add(product);
				return true;
			}
			return false;
		}
	}

	public class ProductGroup
	{
		public List<Items.StandardProduct> Products = new List<Items.StandardProduct>();
	}
}