using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.Web;
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

		public int ProductsPerRow
		{
			get { return (int) (ViewState["ProductsPerRow"] ?? 4); }
			set { ViewState["ProductsPerRow"] = value; }
		}

		public int? PageSize
		{
			get { return ViewState["PageSize"] as int?; }
			set { ViewState["PageSize"] = value; }
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			lsvProducts.GroupItemCount = this.ProductsPerRow;

			if (this.DataSource != null)
			{
				List<ProductGroup> groups = new List<ProductGroup>();
				for (int i = 0, length = this.DataSource.Count(); i < length; i += this.ProductsPerRow)
				{
					ProductGroup group = new ProductGroup();
					for (int j = 0; j < this.ProductsPerRow; j++)
						AddProduct(i + j, group);
					groups.Add(group);
				}
				if (this.PageSize != null)
				{
					int currentPage = 1;
					if (Request.GetOptionalInt("p") != null)
						currentPage = Request.GetRequiredInt("p");

					int pageSize = this.PageSize.Value / this.ProductsPerRow;
					int totalPageCount = (int) Math.Ceiling((double) groups.Count() / pageSize);

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
					plcPageLinks.Visible = true;

					groups = groups.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
				}
				lsvProducts.DataSource = groups;
				lsvProducts.DataBind();

				if (lsvProducts.Items.Count == 0)
					h1Header.Visible = false;
			}
		}

		private void AddProduct(int i, ProductGroup group)
		{
			Items.StandardProduct product = this.DataSource.ElementAtOrDefault(i);
			if (product != null)
			group.Products.Add(product);
		}
	}

	public class ProductGroup
	{
		public List<Items.StandardProduct> Products = new List<Items.StandardProduct>();
	}
}