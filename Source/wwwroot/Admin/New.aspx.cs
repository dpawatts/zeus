using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.ContentTypes;

namespace Zeus.Admin
{
	public partial class New : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			ContentItem parentItem = Zeus.Context.Persister.Get(Convert.ToInt32(Request.QueryString["parentid"]));

			ContentTypeManager manager = new ContentTypeManager();
			lsvChildTypes.DataSource = manager[parentItem.GetType()].AllowedChildren;
			lsvChildTypes.DataBind();
		}
	}
}
