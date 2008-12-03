using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.Definitions;

namespace Zeus.Edit
{
	public partial class New : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			DefinitionManager manager = new DefinitionManager();
			rptItemDefinitions.DataSource = manager.GetDefinitions();
			rptItemDefinitions.DataBind();
		}
	}
}
