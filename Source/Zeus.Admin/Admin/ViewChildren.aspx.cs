using System;
using Isis.ExtensionMethods.Web.UI;
using Isis.Web.Hosting;
using Zeus.ContentTypes;
using Zeus.Web.UI;

[assembly: EmbeddedResourceFile("Zeus.Admin.ViewChildren.aspx", "Zeus.Admin")]
namespace Zeus.Admin
{
	public partial class ViewChildren : PreviewFrameAdminPage
	{
		protected override void OnInit(EventArgs e)
		{
			zeusItemGridView.CurrentItem = SelectedItem;

			ContentType contentType = Zeus.Context.ContentTypes.GetContentType(this.SelectedItem.GetType());
			ltlDiscriminator.Text = Zeus.Context.ContentTypes.GetAllowedChildren(contentType, null, this.User)[0].Discriminator;

			base.OnInit(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			if (!IsPostBack)
			{
				zeusItemGridView.DataSource = SelectedItem.GetChildren();
				zeusItemGridView.DataBind();
			}

			base.OnLoad(e);
		}

		protected void btnRefreshGrid_Click(object sender, EventArgs e)
		{
			zeusItemGridView.DataBind();
			Refresh(SelectedItem, AdminFrame.Navigation, true);
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterJQuery();
			Page.ClientScript.RegisterJavascriptResource(typeof(ViewChildren), "Zeus.Admin.Assets.JS.Plugins.thickbox.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(ViewChildren), "Zeus.Admin.Assets.JS.zeus.js", ResourceInsertPosition.HeaderTop);
			Page.ClientScript.RegisterJavascriptResource(typeof(ViewChildren), "Zeus.Admin.Assets.JS.view.js");
			Page.ClientScript.RegisterCssResource(typeof(ViewChildren), "Zeus.Admin.Assets.Css.shared.css");
			Page.ClientScript.RegisterCssResource(typeof(ViewChildren), "Zeus.Admin.Assets.Css.view.css");
			Page.ClientScript.RegisterCssResource(typeof(ViewChildren), "Zeus.Admin.Assets.Css.thickbox.css");
			base.OnPreRender(e);
		}
	}
}
