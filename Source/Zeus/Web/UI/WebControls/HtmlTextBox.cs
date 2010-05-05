using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Coolite.Ext.UX;
using Ext.Net;
using Zeus.Admin;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.Web.Hosting;
using Panel = Ext.Net.Panel;

namespace Zeus.Web.UI.WebControls
{
	public sealed class HtmlTextBox : WebControl, ITextControl
	{
		private TinyMCE _textBox;

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

		public bool ReadOnly
		{
			get { return (bool) (ViewState["ReadOnly"] ?? false); }
			set { ViewState["ReadOnly"] = value; }
		}

		public string Text
		{
			get
			{
				EnsureChildControls();
				return _textBox.Text;
			}
			set
			{
				EnsureChildControls();
				_textBox.Text = value;
			}
		}

		#endregion

		#region Methods

		protected override void CreateChildControls()
		{
			TinyMCE tinyMce = new TinyMCE();
			tinyMce.ID = ID + "TinyMCE";
			tinyMce.Settings.Mode = "none";
			tinyMce.Settings.Theme = "advanced";
			tinyMce.Settings.Plugins =
				"style,layer,table,advimage,advlink,iespell,media,searchreplace,print,contextmenu,paste,fullscreen,noneditable,inlinepopups";
			tinyMce.Settings.ThemeAdvancedButtons1AddBefore = "";
			tinyMce.Settings.ThemeAdvancedButtons1 =
				"bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect";
			tinyMce.Settings.ThemeAdvancedButtons1Add =
				"styleprops,sup,|,print,fullscreen,|,search,replace,iespell,|,forecolorpicker";
			tinyMce.Settings.ThemeAdvancedButtons2AddBefore = "cut,copy,paste,pastetext,pasteword,|";
			//theme_advanced_buttons2: '',
			tinyMce.Settings.ThemeAdvancedButtons2Add = "|,table,media,insertlayer,inlinepopups";
			tinyMce.Settings.ThemeAdvancedButtons3 = "";
			tinyMce.Settings.ThemeAdvancedButtons3AddBefore = "";
			tinyMce.Settings.ThemeAdvancedButtons3Add = "";
			tinyMce.Settings.ThemeAdvancedButtons4 = "";
			tinyMce.Settings.ThemeAdvancedToolbarLocation = "top";
			tinyMce.Settings.ThemeAdvancedToolbarAlign = "left";
			tinyMce.Settings.ThemeAdvancedPathLocation = "bottom";
			tinyMce.Settings.ExtendedValidElements =
				"hr[class|width|size|noshade],span[class|align|style],pre[class],code[class],iframe[src|width|height|name|align],dynamiccontent[state]";
			tinyMce.Settings.FileBrowserCallback = "fileBrowserCallBack";
			tinyMce.Settings.ThemeAdvancedResizeHorizontal = false;
			tinyMce.Settings.ThemeAdvancedResizing = false;
			tinyMce.Settings.ThemeAdvancedDisable = "help,fontselect,fontsizeselect,forecolor,backcolor";
			tinyMce.Settings.RelativeUrls = false;
			tinyMce.Settings.NonEditableClass = "nonEditable";
			//custom_elements: '~dynamiccontent', // Notice the ~ prefix to force a span element for the element
			tinyMce.Settings.RemoveScriptHost = !DomainAbsoluteUrls;
			tinyMce.Settings.DocumentBaseUrl = Page.Request.Url.GetLeftPart(UriPartial.Authority);
			tinyMce.Settings.ConvertUrls = true;
			tinyMce.Settings.BodyID = RootHtmlElementID;
			if (!string.IsNullOrEmpty(CustomCssUrl))
				tinyMce.Settings.ContentCss = CustomCssUrl;
			if (!string.IsNullOrEmpty(CustomStyleList))
				tinyMce.Settings.ThemeAdvancedStyles = CustomStyleList;
			tinyMce.Width = 600;
			tinyMce.Height = 400;
			tinyMce.ReadOnly = ReadOnly;
			Controls.Add(tinyMce);

			_textBox = tinyMce;

			base.CreateChildControls();
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterJavascriptInclude(Utility.GetClientResourceUrl(GetType(), "TinyMCE/tiny_mce.js"), ResourceInsertPosition.HeaderTop);

			Page.ClientScript.RegisterClientScriptBlock(GetType(), "HtmlTextBox",
				@"function fileBrowserCallBack(fieldName, url, destinationType, win)
				{
					var srcField = win.document.forms[0].elements[fieldName];
					var insertFileUrl = function(data) {
						srcField.value = data.url;
					};

					top.fileManager.show(Ext.get(fieldName), insertFileUrl, destinationType);
				}", true);

			base.OnPreRender(e);
		}

		#endregion
	}
}