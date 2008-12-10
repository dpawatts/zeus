using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.Web;
using Zeus.ContentTypes;

namespace Zeus.Admin
{
	public partial class New : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			ContentItem parentItem = Zeus.Context.Persister.Get(Request.GetRequiredInt("parentid"));

			IContentTypeManager manager = Zeus.Context.Current.Resolve<IContentTypeManager>();
			lsvChildTypes.DataSource = manager[parentItem.GetType()].AllowedChildren;
			lsvChildTypes.DataBind();
		}
	}
}
