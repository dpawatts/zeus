using System;
using System.Web.UI.WebControls;

namespace Zeus.ContentTypes.Properties
{
	public class HtmlTextBox : TextBox
	{
		public HtmlTextBox()
		{
			this.TextMode = TextBoxMode.MultiLine;
			this.DomainAbsoluteUrls = false;
		}

		public bool DomainAbsoluteUrls
		{
			get;
			set;
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			this.Page.ClientScript.RegisterClientScriptInclude("tiny_mce.js", ResolveClientUrl("~/admin/assets/js/tiny_mce/tiny_mce.js"));
			this.Page.ClientScript.RegisterClientScriptInclude("tinymce.js", ResolveClientUrl("~/admin/assets/js/tinymce.js"));
			this.Page.ClientScript.RegisterStartupScript(typeof(HtmlTextBox),
				"InitHtmlTextBox" + this.UniqueID,
				string.Format(@"htmlEditor_init('/Admin/FileManager/default.aspx',
					{{
						elements : '{0}',
						content_css : '/Assets/Css/Core.css',
						remove_script_host : {1},
						document_base_url: '{2}'
					}}
					);", this.ClientID, (!this.DomainAbsoluteUrls).ToString().ToLower(), Page.Request.Url.GetLeftPart(UriPartial.Authority)), true);
		}
	}
}
