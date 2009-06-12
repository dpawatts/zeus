using System.Web.UI;
using System.Web.UI.HtmlControls;
using Isis.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class FlashFileUploadImplementation : FileUploadImplementation
	{
		private HtmlGenericControl _flashControlContainer;

		public FlashFileUploadImplementation(FileDataEditor ownerControl)
			: base(ownerControl)
		{

		}

		public override bool RendersSelf
		{
			get { return true; }
		}

		public override string StartUploadJavascriptFunction
		{
			get { return string.Format("swfobject.getObjectById('{0}').openDialog();", _flashControlContainer.ClientID); }
		}

		public override void AddChildControls()
		{
			_flashControlContainer = new HtmlGenericControl("div") { ID = "flashFileUploader" };
			OwnerControl.Controls.Add(_flashControlContainer);
		}

		public override void Initialize()
		{
			ScriptManager.RegisterClientScriptResource(OwnerControl, OwnerControl.GetType(), "Zeus.Web.Resources.FileDataEditor.swfobject.js");

			string flashVars = string.Format(@"{{
				PercentageChanged : '$(%22#{0}%22).fileDataEditor().onPercentageChanged',
				UploadStarted : '$(%22#{0}%22).fileDataEditor().onUploadStarted',
				UploadFinished : '$(%22#{0}%22).fileDataEditor().onUploadCompleted',
				textColor : '#000000',
				textSize : '12',
				value : '{1}' }}",
				OwnerControl.ClientID,
				OwnerControl.BeforeUploadText);

			string script = string.Format(@"swfobject.embedSWF('{0}', '{1}', '70', '20', '9.0.0', '', {2}, {{ allowScriptAccess: 'always', menu: 'false', bgColor: '#FFFFFF' }}, {{ id: '{1}', name: '{1}', style: 'float: left' }});",
					EmbeddedWebResourceUtility.GetUrl(OwnerControl.GetType().Assembly, "Zeus.Web.Resources.FileDataEditor.FileDataUploader.swf"),
					_flashControlContainer.ClientID,
					flashVars);
			ScriptManager.RegisterStartupScript(OwnerControl,
				OwnerControl.GetType(), OwnerControl.ClientID + "Flash",
				script, true);
		}
	}
}