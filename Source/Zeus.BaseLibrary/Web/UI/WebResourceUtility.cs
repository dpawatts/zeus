using System;
using System.Web;
using System.Web.UI;

namespace Isis.Web.UI
{
	public static class WebResourceUtility
	{
		public static string GetUrl(Type type, string resourceName)
		{
			ClientScriptManager csm;
			if (HttpContext.Current != null && HttpContext.Current.Handler is Page)
				csm = ((Page) HttpContext.Current.Handler).ClientScript;
			else
				csm = new Page().ClientScript;
			return csm.GetWebResourceUrl(type, resourceName);
		}
	}
}