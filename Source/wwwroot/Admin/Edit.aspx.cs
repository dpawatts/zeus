using System;
using System.Web.UI.WebControls;

namespace Zeus.Edit
{
	public partial class Edit : System.Web.UI.Page
	{
		protected void Page_Init(object sender, EventArgs e)
		{
			zeusItemEditor.Discriminator = "News";
		}

		protected void btnSave_Command(object sender, CommandEventArgs e)
		{
			zeusItemEditor.Save();
		}
	}
}
