using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web.UI;
using Isis.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class FancyFileUpload : Control
	{
		#region Fields

		private HiddenField _hiddenFileNameField;
		private string _currentFileName;

		#endregion

		#region Properties

		public string CurrentFileName
		{
			set
			{
				_currentFileName = value;
				EnsureChildControls();
			}
		}

		public string Identifier
		{
			get
			{
				if (ViewState["Identifier"] == null)
					ViewState["Identifier"] = Guid.NewGuid().ToString();
				return ViewState["Identifier"] as string;
			}
		}

		public string TypeFilterDescription
		{
			get { return ViewState["TypeFilterDescription"] as string ?? "Images (*.jpg, *.jpeg, *.gif, *.png)"; }
			set { ViewState["TypeFilterDescription"] = value; }
		}

		public string[] TypeFilter
		{
			get { return ViewState["TypeFilter"] as string[] ?? new[] { "*.jpg", "*.jpeg", "*.gif", "*.png" }; }
			set { ViewState["TypeFilter"] = value; }
		}

		public int MaximumFileSize
		{
			get { return (int) (ViewState["MaximumFileSize"] ?? 5); }
			set { ViewState["MaximumFileSize"] = value; }
		}

		public bool HasNewOrChangedFile
		{
			get { return (!string.IsNullOrEmpty(FileName)) && FileName != "-1"; }
		}

		public bool HasDeletedFile
		{
			get { return FileName == "-1"; }
		}

		public string FileName
		{
			get
			{
				EnsureChildControls();
				return _hiddenFileNameField.Value;
			}
		}

		public bool Enabled
		{
			get { return (bool)(ViewState["Enabled"] ?? true); }
			set { ViewState["Enabled"] = value; }
		}

		private string GetAnchorClientID()
		{
			return ClientID + "DemoAttach";
		}

		private string GetListClientID()
		{
			return ClientID + "DemoList";
		}

		#endregion

		protected override void OnLoad(EventArgs e)
		{
			// TODO: Prevent edit page submission when uploads are in progress.

			base.OnLoad(e);
			if (Page.IsPostBack)
				EnsureChildControls();
		}

		protected override void CreateChildControls()
		{
			_hiddenFileNameField = new HiddenField { ID = ID + "hdnFileName" };
			Controls.Add(_hiddenFileNameField);

			base.CreateChildControls();
		}

		protected override void OnPreRender(EventArgs e)
		{
			string html = string.Format(@"<div style=""float:left;width:600px;margin-bottom:10px;""><a href=""#"" id=""{0}"">Attach a file</a>
				<ul class=""demo-list"" id=""{1}""></ul></div>", GetAnchorClientID(), GetListClientID());
			Controls.Add(new LiteralControl(html));

			Page.ClientScript.RegisterCssResource(GetType(), "Zeus.Web.Resources.FancyFileUpload.FancyFileUpload.css");

			ScriptManager.RegisterClientScriptInclude(this, GetType(), "FancyFileUploadMooTools", 
				"http://ajax.googleapis.com/ajax/libs/mootools/1.2.2/mootools.js");

			ScriptManagerUtility.RegisterEmbeddedClientScriptResource(this, GetType(), "FancyFileUploadFxProgressBar",
				"Zeus.Web.Resources.FancyFileUpload.Fx.ProgressBar.js");

			ScriptManagerUtility.RegisterEmbeddedClientScriptResource(this, GetType(), "FancyFileUploadSwiffUploader",
				"Zeus.Web.Resources.FancyFileUpload.Swiff.Uploader.js");

			Page.ClientScript.RegisterClientScriptResource(GetType(),
				"Zeus.Web.Resources.FancyFileUpload.FancyUpload3.Attach2.js");

			string script = string.Format(@"var {8}up;
window.addEvent('domready', function() {{

	{8}up = new FancyUpload3.Attach('{3}', '{4}', {{
		path: '{0}',
		url: '/PostedFileUpload.axd',
		fileSizeMax: {7} * 1024 * 1024,
		data: 'identifier={5}',
		
		verbose: true,
		multiple: false,
		queued: false,

		typeFilter: {{
			'{1}': '{2}'
		}},
		
		onSelectFail: function(files) {{
			files.each(function(file) {{
				new Element('li', {{
					'class': 'file-invalid',
					events: {{
						click: function() {{
							this.destroy();
						}}
					}}
				}}).adopt(
					new Element('span', {{html: file.validationErrorMessage || file.validationError}})
				).inject(this.list, 'bottom');
			}}, this);
		}},
		
		onFileSuccess: function(file) {{
			var checkbox = new Element('input', {{type: 'checkbox', 'checked': true}});
			checkbox.addEvent('click', function() {{
				file.remove();
				document.getElementById('{6}').value = '';
				return false;
			}}.bind(file));
			checkbox.inject(file.ui.element, 'top');

			file.ui.element.highlight('#e6efc2');
			document.getElementById('{6}').value = file.name;
		}},

		onFileError: function(file) {{
			file.ui.cancel.set('html', 'Retry').removeEvents().addEvent('click', function() {{
				file.requeue();
				return false;
			}});
			
			new Element('span', {{
				html: file.errorMessage,
				'class': 'file-error'
			}}).inject(file.ui.cancel, 'after');
		}},
		
		onFileRequeue: function(file) {{
			file.ui.element.getElement('.file-error').destroy();
			
			file.ui.cancel.set('html', 'Cancel').removeEvents().addEvent('click', function() {{
				file.remove();
				return false;
			}});
			
			this.start();
		}}
		
	}});

}});", EmbeddedWebResourceUtility.GetUrl(GetType().Assembly, "Zeus.Web.Resources.FancyFileUpload.Swiff.Uploader.swf"),
		 TypeFilterDescription,
		 string.Join(";",  TypeFilter),
		 GetListClientID(),
		 GetAnchorClientID(),
		 Identifier,
		 _hiddenFileNameField.ClientID,
		 MaximumFileSize,
		 ClientID);
			ScriptManager.RegisterClientScriptBlock(this, GetType(), ClientID + "FancyFileUpload", script, true);

			if (!string.IsNullOrEmpty(_currentFileName))
			{
				string existingFileScript = string.Format(@"window.addEvent('domready', function() {{
	{2}up.select.setStyle('display', 'none');
	{2}up.reposition();

	var existingUi = {{}};

	existingUi.element = new Element('li', {{ 'class': 'file', id: 'file-1' }});
	existingUi.title = new Element('span', {{ 'class': 'file-title', text: '{0}' }});

	existingUi.element.adopt(
		existingUi.title
	).inject({2}up.list).highlight();

	var checkbox = new Element('input', {{type: 'checkbox', 'checked': true}});
	checkbox.addEvent('click', function() {{
		existingUi.element = existingUi.element.destroy();
		{2}up.onFileRemove();
		document.getElementById('{1}').value = '-1';
		return false;
	}});
	checkbox.inject(existingUi.element, 'top');
}});", _currentFileName, _hiddenFileNameField.ClientID, ClientID);
				ScriptManager.RegisterClientScriptBlock(this, GetType(), ClientID + "FancyFileUploadExistingFile", existingFileScript, true);
			}

			base.OnPreRender(e);
		}
	}
}