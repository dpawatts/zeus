using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeus.Admin
{
	public partial class PreviewFrame : System.Web.UI.MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			this.ID = "master";
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			h2Title.InnerText = Page.Title;
		}
	}
}
