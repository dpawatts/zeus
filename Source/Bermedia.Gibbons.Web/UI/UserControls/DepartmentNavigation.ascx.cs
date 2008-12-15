using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.Web.UI;
using Bermedia.Gibbons.Web.Items;

namespace Bermedia.Gibbons.Web.UI.UserControls
{
	public partial class DepartmentNavigation : System.Web.UI.UserControl
	{
		private BaseDepartment _department;

		public BaseDepartment Department
		{
			get
			{
				if (_department == null)
					_department = ((IContentTemplate) this.Page).CurrentItem.FindFirstAncestor<BaseDepartment>();
				return _department;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				// TODO: Remove ToArray
				foreach (BaseDepartment department in Zeus.Context.Current.Database.ContentItems.ToArray().OfType<BaseDepartment>())
				{
					ListItem item = new ListItem(department.Title, department.ID.ToString());
					ddlSearchDepartment.Items.Add(item);
				}
			}

			rptChildPages.DataBind();
			rptCategories.DataBind();
		}
	}
}