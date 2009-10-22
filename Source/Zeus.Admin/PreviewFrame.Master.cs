using System;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.Web.UI;

namespace Zeus.Admin
{
	public partial class PreviewFrame : System.Web.UI.MasterPage
	{
		protected void Page_Init(object sender, EventArgs e)
		{
			ID = "master";
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterJQuery();

			Page.ClientScript.RegisterJavascriptResource(typeof(PreviewFrame), "Zeus.Admin.Assets.JS.Plugins.jquery.cookies.js", ResourceInsertPosition.HeaderTop);
			Page.ClientScript.RegisterJavascriptResource(typeof(PreviewFrame), "Zeus.Admin.Assets.JS.zeus.js", ResourceInsertPosition.HeaderTop);
			Page.ClientScript.RegisterCssResource(typeof(PreviewFrame), "Zeus.Admin.Assets.Css.reset.css");
			Page.ClientScript.RegisterCssResource(typeof(PreviewFrame), "Zeus.Admin.Assets.Css.shared.css");
			Page.ClientScript.RegisterCssResource(typeof(PreviewFrame), "Zeus.Admin.Assets.Css.preview.css");

			h2Title.InnerText = Page.Title;

			base.OnPreRender(e);
		}
	}
}
