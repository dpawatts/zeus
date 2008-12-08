using System;
using System.Web.UI.WebControls;
using Isis.Web;
using Zeus.ContentTypes;

namespace Zeus.Admin
{
	public partial class Edit : AdminPage
	{
		protected override void OnInit(EventArgs e)
		{
			if (Request.QueryString["discriminator"] != null)
			{
				string discriminator = Request.QueryString["discriminator"];
				zeusItemView.Discriminator = discriminator;
			}

			if (Request.QueryString["parentid"] != null)
				zeusItemView.ParentItemID = Request.GetRequiredInt("parentid");

			if (Request.QueryString["id"] != null)
				zeusItemView.CurrentItem = Zeus.Context.Persister.Get(Request.GetRequiredInt("id"));

			//ContentType definition = Zeus.Context.Current.ContentTypes[discriminator];
			//this.Title = "Edit \"" + definition.ContentTypeAttribute.Title + "\"";

			base.OnInit(e);
		}

		protected void btnSave_Command(object sender, CommandEventArgs e)
		{
			zeusItemView.Save();
			Refresh(zeusItemView.CurrentItem, false);
		}
	}
}
