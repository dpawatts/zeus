using System.Web.UI;
using System.Web.UI.HtmlControls;
using Isis.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class FlashFileUploadImplementation : FileUploadImplementation
	{
		private HtmlGenericControl _flashControlContainer;

		public FlashFileUploadImplementation(Control ownerControl)
			: base(ownerControl)
		{
			
		}

		public override string StartUploadJavascriptFunction
		{
			get { return string.Format("$('#{0}').get(0).openDialog();", _flashControlContainer.ClientID); }
		}

		public override void AddChildControls()
		{
			_flashControlContainer = new HtmlGenericControl("div") { ID = "flashFileUploader" };
			OwnerControl.Controls.Add(_flashControlContainer);
		}

		public override void Initialize()
		{
			ScriptManager.RegisterClientScriptResource(OwnerControl, OwnerControl.GetType(),  "Zeus.Web.Resources.FileDataEditor.swfobject.js");

			string flashVars = string.Format(@"{{
				'PercentageChanged' : '$(""#{0}"").fileDataEditor().onPercentageChanged',
				'UploadStarted' : '$(""#{0}"").fileDataEditor().onUploadStarted',
				'UploadFinished' : '$(""#{0}"").fileDataEditor().onUploadCompleted' }}",
				OwnerControl.ClientID);
			string script = string.Format(@"swfobject.embedSWF('{0}', '{1}', '0', '0', '9.0.0', '', {2}, {{ 'allowScriptAccess' : 'always' }});",
				EmbeddedWebResourceUtility.GetUrl(OwnerControl.GetType().Assembly, "Zeus.Web.Resources.FileDataEditor.FileDataUploader.swf"),
				_flashControlContainer.ClientID,
				flashVars);
			ScriptManager.RegisterStartupScript(OwnerControl,
				OwnerControl.GetType(), OwnerControl.ClientID + "Flash", 
				script, true);
		}
	}
}