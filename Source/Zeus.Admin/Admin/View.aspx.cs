using System;
using Zeus.ContentTypes;

namespace Zeus.Admin
{
	public partial class View : AdminPage
	{
		protected override void OnInit(EventArgs e)
		{
			zeusItemGridView.CurrentItem = this.SelectedItem;

			if (!string.IsNullOrEmpty(Request.QueryString["discriminator"]))
			{
				cdsChildren.OfTypeExact = Zeus.Context.ContentTypes.GetContentType(Request.QueryString["discriminator"]).ItemType.FullName;
				ltlDiscriminator.Text = Request.QueryString["discriminator"];
			}
			else
			{
				ContentType contentType = Zeus.Context.ContentTypes.GetContentType(this.SelectedItem.GetType());
				ltlDiscriminator.Text = Zeus.Context.ContentTypes.GetAllowedChildren(contentType, this.User)[0].Discriminator;
			}

			base.OnInit(e);
		}

		protected void btnRefreshGrid_Click(object sender, EventArgs e)
		{
			zeusItemGridView.DataBind();
			Refresh(this.SelectedItem, AdminFrame.Navigation, true);
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
		}
	}
}
