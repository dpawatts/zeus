using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.Web.UI;
using Zeus.Web;

namespace Bermedia.Gibbons.Web.UI.UserControls
{
	public partial class Search : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				foreach (Items.BaseDepartment department in Zeus.Find.StartPage.GetChildren<Items.BaseDepartment>())
				{
					AddDepartment(department, string.Empty);

					foreach (Items.BaseDepartment childDepartment in department.GetChildren<Items.BaseDepartment>())
						AddDepartment(childDepartment, " » ");
				}
			}
		}

		private void AddDepartment(Items.BaseDepartment department, string titlePrefix)
		{
			ListItem item = new ListItem(titlePrefix + department.Title, department.ID.ToString());
			if (((IContentTemplate) this.Page).CurrentItem.FindFirstAncestor<Items.BaseDepartment>() == department)
				item.Selected = true;
			ddlSearchDepartment.Items.Add(item);
		}

		protected void btnSearch_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(txtSearchText.Text))
				return;

			string url;
			if (!string.IsNullOrEmpty(ddlSearchDepartment.SelectedValue))
				url = new Url(Zeus.Context.Persister.Get<Items.BaseDepartment>(Convert.ToInt32(ddlSearchDepartment.SelectedValue)).Url).AppendSegment("/search").ToString();
			else
				url = "~/search.aspx";
			url += "?q=" + txtSearchText.Text;
			Response.Redirect(url);
		}
	}
}