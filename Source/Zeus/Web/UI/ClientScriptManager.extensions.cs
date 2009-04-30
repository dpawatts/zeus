using System.Web.UI;
using Isis.ExtensionMethods.Web.UI;
using Zeus.Web.UI.WebControls;

namespace Zeus.Web.UI
{
	public static class ClientScriptManagerExtensionMethods
	{
		public static void RegisterJQuery(this ClientScriptManager clientScriptManager)
		{
			clientScriptManager.RegisterJavascriptResource(typeof(HtmlTextBox), "Zeus.Web.Resources.jQuery.jquery.js", ResourceInsertPosition.HeaderTop);
		}
	}
}