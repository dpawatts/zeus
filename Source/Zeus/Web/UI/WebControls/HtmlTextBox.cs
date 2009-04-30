using System;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public sealed class HtmlTextBox : TextBox
	{
		public HtmlTextBox()
		{
			TextMode = TextBoxMode.MultiLine;
		}

		#region Properties

		public bool DomainAbsoluteUrls
		{
			get { return (bool) (ViewState["DomainAbsoluteUrls"] ?? false); }
			set { ViewState["DomainAbsoluteUrls"] = value; }
		}

		public string RootHtmlElementID
		{
			get { return (string) (ViewState["RootHtmlElementID"] ?? string.Empty); }
			set { ViewState["RootHtmlElementID"] = value; }
		}

		public string UploadFolder
		{
			get { return (string) (ViewState["UploadFolder"] ?? "~/Upload"); }
			set { ViewState["UploadFolder"] = value; }
		}

		#endregion

		#region Methods

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			Page.ClientScript.RegisterEmbeddedJavascriptResource(typeof(HtmlTextBox), "Zeus.Web.Resources.TinyMCE.tiny_mce.js", ResourceInsertPosition.HeaderTop);
			Page.ClientScript.RegisterJavascriptResource(typeof(HtmlTextBox), "Zeus.Web.Resources.tinymce.js", ResourceInsertPosition.HeaderTop);
			Page.ClientScript.RegisterStartupScript(typeof(HtmlTextBox),
				"InitHtmlTextBox" + UniqueID,
				string.Format(@"htmlEditor_init('/Admin/FileManager/default.aspx?rootPath={4}',
					{{
						elements: '{0}',
						content_css: '/Assets/Css/Core.css',
						remove_script_host: {1},
						document_base_url: '{2}',
						convert_urls : false,
						body_id: '{3}'
					}}
					);", ClientID,
						 (!DomainAbsoluteUrls).ToString().ToLower(),
						 Page.Request.Url.GetLeftPart(UriPartial.Authority),
						 RootHtmlElementID,
						 Page.Server.UrlEncode(UploadFolder)),
						 true);
		}

		#endregion
	}
}