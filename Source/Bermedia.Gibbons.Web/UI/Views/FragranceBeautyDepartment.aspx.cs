using System;
using System.Linq;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class FragranceBeautyDepartment : ContentPage<Bermedia.Gibbons.Web.Items.FragranceBeautyDepartment>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Trace.Write("Bind fragrance & beauty brands dropdown - Begin");
				var childCategories = Zeus.Find.EnumerateAccessibleChildren(this.CurrentItem).OfType<Web.Items.FragranceBeautyBrandCategory>().ToList();
				Trace.Write("Enumerated child categories");
				var brands = Zeus.Context.Current.Finder.Elements<Web.Items.Brand>().ToList();
				Trace.Write("Retrieved brands from database");
				ddlBrands.DataSource = brands.Where(b => childCategories.Any(c => c.Brand == b));
				ddlBrands.DataBind();
				Trace.Write("Bind fragrance & beauty brands dropdown - End");

				ddlDepartments.DataSource = this.CurrentItem.GetChildren<Web.Items.FragranceBeautyCategory>();
				ddlDepartments.DataBind();

				ddlScents.DataSource = Zeus.Context.Current.Finder.Elements<Web.Items.ProductScent>();
				ddlScents.DataBind();
			}
		}

		protected void btnSearch_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid)
				return;

			Response.Redirect(new Zeus.Web.Url(this.CurrentItem.Url).AppendSegment("search").AppendQuery("q", txtSearchText.Text).ToString());
		}

		protected void btnChooseBrand_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid)
				return;

			Response.Redirect(new Zeus.Web.Url(this.CurrentItem.Url).AppendSegment("brand").AppendQuery("id", ddlBrands.SelectedValue).ToString());
		}

		protected void btnGo2_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid)
				return;

			Response.Redirect(Zeus.Context.Persister.Get(Convert.ToInt32(ddlDepartments.SelectedValue)).Url);
		}

		protected void btnChooseByScent_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid)
				return;

			Zeus.Web.Url url = new Zeus.Web.Url(this.CurrentItem.Url).AppendSegment("scents");
			if (ddlScents.SelectedValue == "-1")
				url = url.AppendQuery("all", "true");
			else
				url = url.AppendQuery("id", ddlScents.SelectedValue);
			Response.Redirect(url.ToString());
		}
	}
}
