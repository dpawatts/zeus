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
					elements: '{0}',
					content_css: '/Assets/Css/Editor.css',
					remove_script_host: {1},
					document_base_url: '{2}',
					convert_urls : false,
					body_id: '{3}'
				}}
				);",
				ClientID,
				(!DomainAbsoluteUrls).ToString().ToLower(),
				Page.Request.Url.GetLeftPart(UriPartial.Authority),
				RootHtmlElementID,
				Page.Server.UrlEncode(UploadFolder),
				Zeus.Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(
					Zeus.Context.Current.Resolve<IAdminAssemblyManager>().Assembly, "Zeus.Admin.FileManager.Default.aspx"));

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