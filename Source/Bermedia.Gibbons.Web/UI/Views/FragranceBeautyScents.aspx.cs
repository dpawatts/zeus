using System;
using System.Linq;
using Isis.ExtensionMethods.Web;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.UI.Views
{
	public partial class FragranceBeautyScents : ContentPage<Bermedia.Gibbons.Web.Items.FragranceBeautyDepartment>
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				var results = Zeus.Context.Current.Finder
					.Elements<Web.Items.FragranceBeautyProduct>()
					.ToList()
					.Where(p => p.Scent != null);

				if (Request.QueryString["id"] != null)
					results = results.Where(p => p.Scent == Zeus.Context.Persister.Get<Web.Items.ProductScent>(Request.GetRequiredInt("id")));

				rptScents.DataSource = results
					.OrderBy(p => p.Title)
					.OrderBy(p => p.Brand.Title)
					.OrderBy(p => (p.Strength != null) ? p.Strength.SortOrder : 0)
					.GroupBy(p => p.Scent)
					.OrderBy(g => g.Key.SortOrder);
				rptScents.DataBind();
			}
		}
	}
}
