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
	public partial class New : AdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			ContentItem parentItem = this.SelectedItem;
			IContentTypeManager manager = Zeus.Context.Current.Resolve<IContentTypeManager>();
			ContentType contentType = manager.GetContentType(parentItem.GetType());
			lsvChildTypes.DataSource = manager.GetAllowedChildren(contentType, this.User);
			lsvChildTypes.DataBind();
		}
	}
}
