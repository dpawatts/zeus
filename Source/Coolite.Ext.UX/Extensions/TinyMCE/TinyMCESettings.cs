using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Ext.Net;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Coolite.Ext.UX
{
	public class TinyMCESettings : StateManagedItem
	{
		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption]
		public virtual string Mode
		{
			get { return (string) ViewState["Mode"] ?? string.Empty; }
			set { ViewState["Mode"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption]
		public virtual string Theme
		{
			get { return (string) ViewState["Theme"] ?? string.Empty; }
			set { ViewState["Theme"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption]
		public virtual string Plugins
		{
			get { return (string) ViewState["Plugins"] ?? string.Empty; }
			set { ViewState["Plugins"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_buttons1_add_before")]
		public virtual string ThemeAdvancedButtons1AddBefore
		{
			get { return (string) ViewState["ThemeAdvancedButtons1AddBefore"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedButtons1AddBefore"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_buttons1")]
		public virtual string ThemeAdvancedButtons1
		{
			get { return (string) ViewState["ThemeAdvancedButtons1"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedButtons1"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_buttons1_add")]
		public virtual string ThemeAdvancedButtons1Add
		{
			get { return (string) ViewState["ThemeAdvancedButtons1Add"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedButtons1Add"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_buttons2_add_before")]
		public virtual string ThemeAdvancedButtons2AddBefore
		{
			get { return (string) ViewState["ThemeAdvancedButtons2AddBefore"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedButtons2AddBefore"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_buttons2")]
		public virtual string ThemeAdvancedButtons2
		{
			get { return (string) ViewState["ThemeAdvancedButtons2"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedButtons2"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_buttons2_add")]
		public virtual string ThemeAdvancedButtons2Add
		{
			get { return (string) ViewState["ThemeAdvancedButtons2Add"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedButtons2Add"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_buttons3_add_before")]
		public virtual string ThemeAdvancedButtons3AddBefore
		{
			get { return (string) ViewState["ThemeAdvancedButtons3AddBefore"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedButtons3AddBefore"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_buttons3")]
		public virtual string ThemeAdvancedButtons3
		{
			get { return (string) ViewState["ThemeAdvancedButtons3"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedButtons3"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_buttons3_add")]
		public virtual string ThemeAdvancedButtons3Add
		{
			get { return (string) ViewState["ThemeAdvancedButtons3Add"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedButtons3Add"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_buttons4")]
		public virtual string ThemeAdvancedButtons4
		{
			get { return (string) ViewState["ThemeAdvancedButtons4"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedButtons4"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_toolbar_location")]
		public virtual string ThemeAdvancedToolbarLocation
		{
			get { return (string) ViewState["ThemeAdvancedToolbarLocation"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedToolbarLocation"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_toolbar_align")]
		public virtual string ThemeAdvancedToolbarAlign
		{
			get { return (string) ViewState["ThemeAdvancedToolbarAlign"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedToolbarAlign"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_path_location")]
		public virtual string ThemeAdvancedPathLocation
		{
			get { return (string) ViewState["ThemeAdvancedPathLocation"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedPathLocation"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_statusbar_location")]
		public virtual string ThemeAdvancedStatusBarLocation
		{
			get { return (string) ViewState["ThemeAdvancedStatusBarLocation"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedStatusBarLocation"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("extended_valid_elements")]
		public virtual string ExtendedValidElements
		{
			get { return (string) ViewState["ExtendedValidElements"] ?? string.Empty; }
			set { ViewState["ExtendedValidElements"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("file_browser_callback")]
		public virtual string FileBrowserCallback
		{
			get { return (string) ViewState["FileBrowserCallback"] ?? string.Empty; }
			set { ViewState["FileBrowserCallback"] = value; }
		}

		[DefaultValue(false)]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_resize_horizontal")]
		public virtual bool ThemeAdvancedResizeHorizontal
		{
			get { return (bool) (ViewState["ThemeAdvancedResizeHorizontal"] ?? false); }
			set { ViewState["ThemeAdvancedResizeHorizontal"] = value; }
		}

		[DefaultValue(true)]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_resizing")]
		public virtual bool ThemeAdvancedResizing
		{
			get { return (bool) (ViewState["ThemeAdvancedResizing"] ?? true); }
			set { ViewState["ThemeAdvancedResizing"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_disable")]
		public virtual string ThemeAdvancedDisable
		{
			get { return (string) ViewState["ThemeAdvancedDisable"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedDisable"] = value; }
		}

		[DefaultValue(false)]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("relative_urls")]
		public virtual bool RelativeUrls
		{
			get { return (bool) (ViewState["RelativeUrls"] ?? false); }
			set { ViewState["RelativeUrls"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("noneditable_noneditable_class")]
		public virtual string NonEditableClass
		{
			get { return (string) ViewState["NonEditableClass"] ?? string.Empty; }
			set { ViewState["NonEditableClass"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("custom_elements")]
		public virtual string CustomElements
		{
			get { return (string) ViewState["CustomElements"] ?? string.Empty; }
			set { ViewState["CustomElements"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("template_external_list_url")]
		public virtual string TemplateExternalListUrl
		{
			get { return (string) ViewState["ExtendedValidElements"] ?? string.Empty; }
			set { ViewState["ExtendedValidElements"] = value; }
		}

		[DefaultValue(false)]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("remove_script_host")]
		public virtual bool RemoveScriptHost
		{
			get { return (bool) (ViewState["RemoveScriptHost"] ?? false); }
			set { ViewState["RemoveScriptHost"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("document_base_url")]
		public virtual string DocumentBaseUrl
		{
			get { return (string) ViewState["DocumentBaseUrl"] ?? string.Empty; }
			set { ViewState["DocumentBaseUrl"] = value; }
		}

		[DefaultValue(false)]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("convert_urls")]
		public virtual bool ConvertUrls
		{
			get { return (bool) (ViewState["ConvertUrls"] ?? false); }
			set { ViewState["ConvertUrls"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("body_id")]
		public virtual string BodyID
		{
			get { return (string) ViewState["BodyID"] ?? string.Empty; }
			set { ViewState["BodyID"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("content_css")]
		public virtual string ContentCss
		{
			get { return (string) ViewState["ContentCss"] ?? string.Empty; }
			set { ViewState["ContentCss"] = value; }
		}

		[DefaultValue("")]
		//[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		//[Category("Config Options")]
		[ConfigOption("theme_advanced_styles")]
		public virtual string ThemeAdvancedStyles
		{
			get { return (string) ViewState["ThemeAdvancedStyles"] ?? string.Empty; }
			set { ViewState["ThemeAdvancedStyles"] = value; }
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[XmlIgnore]
		[JsonIgnore]
		public override ConfigOptionsCollection ConfigOptions
		{
			get
			{
				ConfigOptionsCollection list = base.ConfigOptions;

				list.Add("custom_elements", string.Empty, CustomElements);
				list.Add("extended_valid_elements", string.Empty, ExtendedValidElements);
				list.Add("file_browser_callback", string.Empty, FileBrowserCallback);
				list.Add("mode", string.Empty, Mode);
				list.Add("noneditable_noneditable_class", string.Empty, NonEditableClass);
				list.Add("plugins", string.Empty, Plugins);
				list.Add("relative_urls", true, RelativeUrls);
				list.Add("template_external_list_url", string.Empty, TemplateExternalListUrl);
				list.Add("theme", string.Empty, Theme);
				list.Add("theme_advanced_buttons1", string.Empty, ThemeAdvancedButtons1);
				list.Add("theme_advanced_buttons1_add", string.Empty, ThemeAdvancedButtons1Add);
				list.Add("theme_advanced_buttons1_add_before", string.Empty, ThemeAdvancedButtons1AddBefore);
				list.Add("theme_advanced_buttons2", string.Empty, ThemeAdvancedButtons2);
				list.Add("theme_advanced_buttons2_add", string.Empty, ThemeAdvancedButtons2Add);
				list.Add("theme_advanced_buttons2_add_before", string.Empty, ThemeAdvancedButtons2AddBefore);
				list.Add("theme_advanced_buttons3", string.Empty, ThemeAdvancedButtons3);
				list.Add("theme_advanced_buttons3_add", string.Empty, ThemeAdvancedButtons3Add);
				list.Add("theme_advanced_buttons3_add_before", string.Empty, ThemeAdvancedButtons3AddBefore);
				list.Add("theme_advanced_buttons4", string.Empty, ThemeAdvancedButtons4);
				list.Add("theme_advanced_disable", string.Empty, ThemeAdvancedDisable);
				list.Add("theme_advanced_path_location", string.Empty, ThemeAdvancedPathLocation);
				list.Add("theme_advanced_reize_horizontal", string.Empty, ThemeAdvancedResizeHorizontal);
				list.Add("theme_advanced_resizing", string.Empty, ThemeAdvancedResizing);
				list.Add("theme_advanced_statusbar_location", string.Empty, ThemeAdvancedStatusBarLocation);
				list.Add("theme_advanced_toolbar_align", string.Empty, ThemeAdvancedToolbarAlign);
				list.Add("theme_advanced_toolbar_location", string.Empty, ThemeAdvancedToolbarLocation);
				list.Add("remove_script_host", true, RemoveScriptHost);
				list.Add("document_base_url", string.Empty, DocumentBaseUrl);
				list.Add("convert_urls", true, ConvertUrls);
				list.Add("body_id", string.Empty, BodyID);
				list.Add("content_css", string.Empty, ContentCss);
				list.Add("theme_advanced_styles", string.Empty, ThemeAdvancedStyles);

				return list;
			}
		}
	}
}