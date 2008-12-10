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
				zeusItemEditView.Discriminator = discriminator;
			}

			if (Request.QueryString["parentid"] != null)
				zeusItemEditView.ParentItemID = Request.GetRequiredInt("parentid");

			if (Request.QueryString["id"] != null)
			{
				zeusItemEditView.CurrentItem = Zeus.Context.Persister.Get(Request.GetRequiredInt("id"));
				this.Title = "Edit \"" + zeusItemEditView.CurrentItem.Title + "\"";
			}
			else
			{
				this.Title = "New " + zeusItemEditView.CurrentItemDefinition.ContentTypeAttribute.Title;
			}

			base.OnInit(e);
		}

		protected void btnSave_Command(object sender, CommandEventArgs e)
		{
			zeusItemEditView.Save();
			Refresh(zeusItemEditView.CurrentItem, false);
		}
	}
}
