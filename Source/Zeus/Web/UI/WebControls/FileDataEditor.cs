using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.Web.UI;
using Zeus.Admin;

namespace Zeus.Web.UI.WebControls
{
	public class FileDataEditor : Control
	{
		#region Fields

		private HtmlGenericControl _silverlightControl, _beforeUpload, _duringUpload, _afterUpload;
		private HiddenField _hiddenFileNameField, _hiddenIdentifierField;
		private HtmlAnchor _beforeUploadAnchor;
		private string _currentFileName;

		#endregion

		//private CheckBox chkClearFile;

		/*#region Properties

		public int ContentID
		{
			get { return (int) (this.ViewState["ContentID"] ?? 0); }
			set { this.ViewState["ContentID"] = value; }
		}

		public bool ShouldClear
		{
			get { return (chkClearFile != null && chkClearFile.Checked); }
		}

		#endregion*/

		public string CurrentFileName
		{
			set
			{
				_currentFileName = value;
				EnsureChildControls();
			}
		}

		public bool Enabled
		{
			get { return (bool) (ViewState["Enabled"] ?? true); }
			set { ViewState["Enabled"] = value; }
		}

		public string FileName
		{
			get
			{
				EnsureChildControls();
				return _hiddenFileNameField.Value;
			}
		}

		public string Identifier
		{
			get
			{
				EnsureChildControls();
				return _hiddenIdentifierField.Value;
			}
		}

		public bool HasNewOrChangedFile
		{
			get { return (!string.IsNullOrEmpty(FileName)); }
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (Page.IsPostBack)
				EnsureChildControls();
		}

		protected override void CreateChildControls()
		{
			_hiddenFileNameField = new HiddenField { ID = ID + "hdnFileName" };
			Controls.Add(_hiddenFileNameField);

			_hiddenIdentifierField = new HiddenField { ID = ID + "hdnIdentifier" };
			Controls.Add(_hiddenIdentifierField);

			HtmlGenericControl silverlightControlHost = new HtmlGenericControl("div");
			Controls.Add(silverlightControlHost);

			_silverlightControl = new HtmlGenericControl("object");
			silverlightControlHost.Controls.Add(_silverlightControl);
			_silverlightControl.ID = ID + "slvUpload";
			_silverlightControl.Attributes["data"] = "data:application/x-silverlight-2,";
			_silverlightControl.Attributes["type"] = "application/x-silverlight-2";
			_silverlightControl.Attributes["width"] = "0";
			_silverlightControl.Attributes["height"] = "0";

			AddParam(_silverlightControl, "source", EmbeddedWebResourceUtility.GetUrl(Zeus.Context.Current.Resolve<IAdminAssemblyManager>().Assembly, "Zeus.Admin.ClientBin.Zeus.SilverlightUpload.xap"));
			AddParam(_silverlightControl, "onerror", "onSilverlightError");
			AddParam(_silverlightControl, "background", "white");
			AddParam(_silverlightControl, "minRuntimeVersion", "2.0.31005.0");
			AddParam(_silverlightControl, "autoUpgrade", "true");

			HtmlGenericControl iframe = new HtmlGenericControl("iframe");
			silverlightControlHost.Controls.Add(iframe);
			iframe.Style[HtmlTextWriterStyle.Visibility] = "hidden";
			iframe.Style[HtmlTextWriterStyle.Height] = "0";
			iframe.Style[HtmlTextWriterStyle.Width] = "0";
			iframe.Style[HtmlTextWriterStyle.BorderWidth] = "0";

			_beforeUpload = new HtmlGenericControl("div") { ID = ID + "beforeUpload" };
			Controls.Add(_beforeUpload);

			if (!string.IsNullOrEmpty(_currentFileName))
				_beforeUpload.Controls.Add(new LiteralControl(_currentFileName + "&nbsp;"));

			_beforeUploadAnchor = new HtmlAnchor { Disabled = !Enabled };
			_beforeUpload.Controls.Add(_beforeUploadAnchor);
			_beforeUploadAnchor.HRef = "#";
			if (!string.IsNullOrEmpty(_currentFileName))
				_beforeUploadAnchor.InnerText = "Change";
			else
				_beforeUploadAnchor.InnerText = "Upload file";

			_duringUpload = new HtmlGenericControl("div") { ID = ID + "duringUpload", InnerText = "Uploading..." };
			Controls.Add(_duringUpload);
			_duringUpload.Style[HtmlTextWriterStyle.Display] = "none";

			_afterUpload = new HtmlGenericControl("div") { ID = ID + "afterUpload", InnerText = "Finished Upload", Disabled = !Enabled };
			Controls.Add(_afterUpload);

			/*
				<!-- shown during upload -->
				<div id="duringUpload">Uploading MyFile.pdf: <span id="percentage">0</span>%</div>

				<!-- shown after upload -->
				<div id="afterUpload">Uploaded: MyFile.pdf (<a href="#">remove</a>)</div>
			 * */

			/*this.Controls.Add(new LiteralControl("<br />"));

			if (this.ContentID != 0)
			{
				File file = Zeus.Context.Persister.Get<File>(this.ContentID);

				Span span = new Span { CssClass = "fileName" };
				span.Controls.Add(new Image { ImageUrl = file.IconUrl });
				span.Controls.Add(new LiteralControl(" " + file.Name));
				this.Controls.Add(span);

				this.Controls.Add(new LiteralControl("<br />"));

				chkClearFile = new CheckBox { ID = "chkClearFile", Text = "Clear", CssClass = "clearFile" };
				this.Controls.Add(chkClearFile);
			}*/
		}

		private static void AddParam(Control silverlightControl, string name, string value)
		{
			HtmlGenericControl param = new HtmlGenericControl("param");
			silverlightControl.Controls.Add(param);
			param.Attributes["name"] = name;
			param.Attributes["value"] = value;
		}

		protected override void OnPreRender(EventArgs e)
		{
			if (!string.IsNullOrEmpty(_hiddenFileNameField.Value))
				_beforeUpload.Style[HtmlTextWriterStyle.Display] = "none";

			if (string.IsNullOrEmpty(_hiddenFileNameField.Value))
				_afterUpload.Style[HtmlTextWriterStyle.Display] = "none";
			else
			{
				_afterUpload.Style[HtmlTextWriterStyle.Display] = string.Empty;
				_afterUpload.InnerText = _hiddenFileNameField.Value;
			}

			_beforeUploadAnchor.Attributes["onclick"] = "document.getElementById('" + _silverlightControl.ClientID + "').Content.Control.SelectFileAndUpload();return false;";

			AddParam(_silverlightControl, "onload", _silverlightControl.ClientID + "pluginLoaded");

			ScriptManager.RegisterClientScriptResource(this, typeof(FileDataEditor), "Zeus.Web.Resources.FileDataEditor.js");
			ScriptManager.RegisterClientScriptResource(this, typeof(FileDataEditor), "Zeus.Web.Resources.Silverlight.js");

			string script = string.Format(@"
    	function {0}pluginLoaded(sender) {{
    		var silverlightControl = document.getElementById('{0}');

    		silverlightControl.Content.Control.MaximumFileSizeReached = function(sender, e) {{
    			alert('Maximum file size exceeded');
    		}};

    		silverlightControl.Content.Control.PercentageChanged = function(sender, e) {{
    			$('#{2}').text('Uploading: ' + e.Percentage + '%');
    		}};

    		silverlightControl.Content.Control.UploadStarted = function(sender, e) {{
    			onUploadStarted();
					$('#{4}').val(e.FileName);
					$('#{5}').val(e.Identifier);
    			$('#{1}').hide();
    			$('#{2}').show();
    		}};

    		silverlightControl.Content.Control.UploadFinished = function(sender, e) {{
    			onUploadCompleted();
    			$('#{2}').hide();
    			$('#{3}').text(e.FileName).show();
    		}};
    	}}", _silverlightControl.ClientID, _beforeUpload.ClientID, _duringUpload.ClientID, _afterUpload.ClientID, _hiddenFileNameField.ClientID, _hiddenIdentifierField.ClientID);
			ScriptManager.RegisterClientScriptBlock(this, GetType(), ClientID, script, true);

			const string startupScript = "if (!Silverlight.isInstalled('2.0.31005.0')) alert('The Silverlight plugin is required to use this page. You can get the plugin here:\\nhttp://www.microsoft.com/silverlight/resources/install.aspx');";
			ScriptManager.RegisterStartupScript(this, GetType(), "SilverlightCheck", startupScript, true);

			base.OnPreRender(e);
		}
	}
}
