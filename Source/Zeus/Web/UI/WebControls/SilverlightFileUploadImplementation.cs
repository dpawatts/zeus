using System.Web.UI;
using System.Web.UI.HtmlControls;
using Isis.Web.UI;
using Zeus.Admin;

namespace Zeus.Web.UI.WebControls
{
	public class SilverlightFileUploadImplementation : FileUploadImplementation
	{
		private HtmlGenericControl _silverlightControl;

		public SilverlightFileUploadImplementation(Control ownerControl)
			: base(ownerControl)
		{
			
		}

		public override string StartUploadJavascriptFunction
		{
			get { return "document.getElementById('" + _silverlightControl.ClientID + "').Content.Control.SelectFileAndUpload();"; }
		}

		public override void AddChildControls()
		{
			HtmlGenericControl silverlightControlHost = new HtmlGenericControl("div");
			OwnerControl.Controls.Add(silverlightControlHost);

			_silverlightControl = new HtmlGenericControl("object");
			silverlightControlHost.Controls.Add(_silverlightControl);
			_silverlightControl.ID = OwnerControl.ID + "slvUpload";
			_silverlightControl.Attributes["data"] = "data:application/x-silverlight-2,";
			_silverlightControl.Attributes["type"] = "application/x-silverlight-2";
			_silverlightControl.Attributes["width"] = "0";
			_silverlightControl.Attributes["height"] = "0";

			AddParam(_silverlightControl, "source", EmbeddedWebResourceUtility.GetUrl(Context.Current.Resolve<IAdminAssemblyManager>().Assembly, "Zeus.Admin.ClientBin.Zeus.SilverlightUpload.xap"));
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
		}

		private static void AddParam(Control silverlightControl, string name, string value)
		{
			HtmlGenericControl param = new HtmlGenericControl("param");
			silverlightControl.Controls.Add(param);
			param.Attributes["name"] = name;
			param.Attributes["value"] = value;
		}

		public override void Initialize()
		{
			AddParam(_silverlightControl, "onload", _silverlightControl.ClientID + "pluginLoaded");

			ScriptManager.RegisterClientScriptResource(OwnerControl, OwnerControl.GetType(), "Zeus.Web.Resources.FileDataEditor.Silverlight.js");
			ScriptManager.RegisterClientScriptResource(OwnerControl, OwnerControl.GetType(), "Zeus.Web.Resources.FileDataEditor.SilverlightFileDataEditor.js");

			string script = string.Format(@"
    	function {0}pluginLoaded(sender) {{
    		var silverlightControl = document.getElementById('{0}');

    		silverlightControl.Content.Control.MaximumFileSizeReached = function(sender, e) {{
    			$('#{1}').fileDataEditor().onMaximumFileSizeReached();
    		}};

    		silverlightControl.Content.Control.PercentageChanged = function(sender, e) {{
					$('#{1}').fileDataEditor().onPercentageChanged(e.Percentage);
    		}};

    		silverlightControl.Content.Control.UploadStarted = function(sender, e) {{
					$('#{1}').fileDataEditor().onUploadStarted(e.FileName, e.Identifier);
    		}};

    		silverlightControl.Content.Control.UploadFinished = function(sender, e) {{
					$('#{1}').fileDataEditor().onUploadCompleted(e.FileName);
    		}};
    	}}", _silverlightControl.ClientID, OwnerControl.ClientID);
			ScriptManager.RegisterClientScriptBlock(OwnerControl,
				OwnerControl.GetType(), OwnerControl.ClientID + "Silverlight", 
				script, true);

			const string STARTUP_SCRIPT = "if (!Silverlight.isInstalled('2.0.31005.0')) alert('The Silverlight plugin is required to use this page. You can get the plugin here:\\nhttp://www.microsoft.com/silverlight/resources/install.aspx');";
			ScriptManager.RegisterStartupScript(OwnerControl, 
				OwnerControl.GetType(), "SilverlightCheck", 
				STARTUP_SCRIPT, true);
		}
	}
}