using System;
using System.Web.UI.WebControls;
using Zeus.ContentTypes;

namespace Zeus.Admin
{
	public partial class Edit : AdminPage
	{
		protected override void OnInit(EventArgs e)
		{
			string discriminator = Request.QueryString["discriminator"];
			zeusItemEditor.Discriminator = discriminator;

			if (Request.QueryString["parentid"] != null)
				zeusItemEditor.ParentItemID = Convert.ToInt32(Request.QueryString["parentid"]);

			ContentType definition = Zeus.Context.Current.ContentTypes[discriminator];
			this.Title = "Edit \"" + definition.DefinitionAttribute.Title + "\"";

			base.OnInit(e);
		}

		protected void btnSave_Command(object sender, CommandEventArgs e)
		{
			zeusItemEditor.Save();
			Refresh(zeusItemEditor.CurrentItem, false);
		}
	}
}
