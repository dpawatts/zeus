using System;
using System.Web.UI.WebControls;
using Coolite.Ext.Web;
using Zeus.Admin;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.Web.Hosting;

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

		/// <summary>
		/// This option enables you to specify a custom CSS file that extends the theme content CSS. This CSS 
		/// file is the one used within the editor (the editable area). This option can also be a comma 
		/// separated list of URLs. 
		/// </summary>
		public string CustomCssUrl
		{
			get { return (string) (ViewState["CustomCssUrl"] ?? "/Assets/Css/Editor.css"); }
			set { ViewState["CustomCssUrl"] = value; }
		}

		/// <summary>
		/// This option can contain a semicolon separated list of class titles and class names separated by =.
		/// The titles will be presented to the user in the styles dropdown list and the class names will 
		/// be inserted. If this option is not defined, TinyMCE imports the classes from the content_css.
		/// </summary>
		public string CustomStyleList
		{
			get { return (string) (ViewState["CustomStyleList"] ?? string.Empty); }
			set { ViewState["CustomStyleList"] = value; }
		}

		#endregion

		#region Methods

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			Page.ClientScript.RegisterJavascriptInclude(Utility.GetClientResourceUrl(GetType(), "TinyMCE/tiny_mce.js"), ResourceInsertPosition.HeaderBottom);
			Page.ClientScript.RegisterJavascriptResource(typeof(HtmlTextBox), "Zeus.Web.Resources.miframe.js", ResourceInsertPosition.HeaderBottom);
			Page.ClientScript.RegisterJavascriptResource(typeof(HtmlTextBox), "Zeus.Web.Resources.tinymce.js", ResourceInsertPosition.HeaderBottom);

			string script = string.Format(
				@"htmlEditor_init('{5}?rootPath={4}',
				{{
					remove_script_host: {1}
					{6}
					, document_base_url: '{2}',
					convert_urls : false,
					body_id: '{3}'
					{7}
				}}, '{0}'
				);",
				ClientID,
				(!DomainAbsoluteUrls).ToString().ToLower(),
				Page.Request.Url.GetLeftPart(UriPartial.Authority),
				RootHtmlElementID,
				Page.Server.UrlEncode(UploadFolder),
				Zeus.Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(
					Zeus.Context.Current.Resolve<IAdminAssemblyManager>().Assembly, "Zeus.Admin.FileManager.Default.aspx"),
				!string.IsNullOrEmpty(CustomCssUrl) ? ", content_css: '" + CustomCssUrl + "'" : null,
				!string.IsNullOrEmpty(CustomStyleList) ? ", theme_advanced_styles: '" + CustomStyleList + "'" : null);

			// If this control is within an ExtJS Tab that is initially hidden, we need to register the script in the TabChanged
			// event instead of on the initial window load.
			Tab parentTab = this.FindParent<Tab>();
			if (parentTab != null && parentTab.ParentComponent is TabPanel && ((TabPanel) parentTab.ParentComponent).ActiveTab != parentTab)
			{
				Page.ClientScript.RegisterStartupScript(typeof(HtmlTextBox),
					"InitHtmlTextBox" + UniqueID,
					string.Format("var {0}Initialised = false;", ClientID),
					true);
				parentTab.Listeners.Activate.Handler = string.Format(
					"if (!{0}Initialised) {{ {1} {0}Initialised = true; }}",
					ClientID, script);
			}
			else
			{
				Page.ClientScript.RegisterStartupScript(typeof(HtmlTextBox),
					"InitHtmlTextBox" + UniqueID,
					script, true);
			}
		}

		#endregion
	}
}