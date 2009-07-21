using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.UI;
using Isis.Web.UI;

namespace Zeus.Web.Mvc.Html
{
	public static class HtmlHelperExtensions
	{
		public static string IncludeJavascriptResource(this HtmlHelper html, Type type, string resourceName)
		{
			return string.Format(@"<script type=""text/javascript"" src=""{0}""></script>",
				WebResourceUtility.GetUrl(type, resourceName));
		}

		public static string IncludeEmbeddedJavascriptResource(this HtmlHelper html, Assembly assembly, string resourceName)
		{
			return string.Format(@"<script type=""text/javascript"" src=""{0}""></script>",
				EmbeddedWebResourceUtility.GetUrl(assembly, resourceName));
		}
	}
}