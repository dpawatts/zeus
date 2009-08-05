using System;
using System.Web.UI;

namespace Isis.Web.UI
{
	public static class ScriptManagerUtility
	{
		public static void RegisterEmbeddedClientScriptResource(Control control, Type type, string key, string resourceName)
		{
			string javascriptUrl = EmbeddedWebResourceUtility.GetUrl(type.Assembly, resourceName);
			ScriptManager.RegisterClientScriptInclude(control, type, key, javascriptUrl);
		}
	}
}