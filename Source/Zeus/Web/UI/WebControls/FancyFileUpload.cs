using System;
using System.Web.UI;
using Isis.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class FancyFileUpload : Control
	{
		protected override void OnPreRender(EventArgs e)
		{
			ScriptManager.RegisterClientScriptInclude(this, GetType(), "FancyFileUploadMooTools", 
				"http://ajax.googleapis.com/ajax/libs/mootools/1.2.2/mootools.js");

			ScriptManagerUtility.RegisterEmbeddedClientScriptResource(this, GetType(), "FancyFileUploadFxProgressBar",
				"Zeus.Web.Resources.FancyFileUpload.Fx.ProgressBar.js");

			ScriptManagerUtility.RegisterEmbeddedClientScriptResource(this, GetType(), "FancyFileUploadSwiffUploader",
				"Zeus.Web.Resources.FancyFileUpload.Swiff.Uploader.js");

			ScriptManagerUtility.RegisterEmbeddedClientScriptResource(this, GetType(), "FancyFileUploadSwiffUploader",
				"Zeus.Web.Resources.FancyFileUpload.FancyUpload3.Attach.js");

			base.OnPreRender(e);
		}
	}
}