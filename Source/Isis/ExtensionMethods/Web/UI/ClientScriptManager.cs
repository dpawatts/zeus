using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Isis.Web.UI;

namespace Isis.ExtensionMethods.Web.UI
{
	public static class ClientScriptManagerExtensionMethods
	{
		public static void RegisterCssInclude(this ClientScriptManager clientScriptManager, string cssUrl)
		{
			if (cssUrl.StartsWith("~"))
				cssUrl = VirtualPathUtility.ToAbsolute(cssUrl);

			Page page = (Page)HttpContext.Current.Handler;
			if (page.Items[cssUrl] == null)
			{
				HtmlLink link = new HtmlLink();
				link.Href = cssUrl;
				link.Attributes["type"] = "text/css";
				link.Attributes["rel"] = "stylesheet";
				link.Attributes["title"] = "Default Style";

				int index = (int)(page.Items["__CssResourceIndex"] ?? 0);
				page.Header.Controls.AddAt(index, link);
				page.Items["__CssResourceIndex"] = ++index;

				page.Items[cssUrl] = new object();
			}
		}

		public static void RegisterCssResource(this ClientScriptManager clientScriptManager, Type type, string resourceName)
		{
			string cssUrl = clientScriptManager.GetWebResourceUrl(type, resourceName);
			RegisterCssInclude(clientScriptManager, cssUrl);
		}

		public static void RegisterEmbeddedCssResource(this ClientScriptManager clientScriptManager, Type type, string resourceName)
		{
			string cssUrl = EmbeddedWebResourceUtility.GetUrl(type.Assembly, resourceName);
			RegisterCssInclude(clientScriptManager, cssUrl);
		}

		public static void RegisterJavascriptInclude(this ClientScriptManager clientScriptManager, string javascriptUrl)
		{
			RegisterJavascriptInclude(clientScriptManager, javascriptUrl, ResourceInsertPosition.HeaderBottom);
		}

		public static void RegisterJavascriptInclude(this ClientScriptManager clientScriptManager, string javascriptUrl, ResourceInsertPosition position)
		{
			RegisterJavascriptInclude(clientScriptManager, javascriptUrl, position, string.Empty, string.Empty);
		}

		public static void RegisterJavascriptInclude(this ClientScriptManager clientScriptManager, string javascriptUrl, ResourceInsertPosition position,
			string prefix, string postfix)
		{
			if (javascriptUrl.StartsWith("~"))
				javascriptUrl = VirtualPathUtility.ToAbsolute(javascriptUrl);

			Page page = (Page)HttpContext.Current.Handler;
			if (page.Items[javascriptUrl] == null)
			{
				string script = string.Format(@"{0}<script src=""{1}"" type=""text/javascript""></script>{2}{3}", prefix, javascriptUrl, postfix, Environment.NewLine);
				LiteralControl jsControl = new LiteralControl(script);

				switch (position)
				{
					case ResourceInsertPosition.HeaderTop:
						{
							int index = (int)(page.Items["__ScriptResourceIndex"] ?? 0);
							page.Header.Controls.AddAt(index, jsControl);
							page.Items["__ScriptResourceIndex"] = ++index;
						}
						break;
					case ResourceInsertPosition.HeaderBottom:
						page.Header.Controls.Add(jsControl);
						break;
					case ResourceInsertPosition.BodyBottom:
						page.Form.Controls.Add(jsControl);
						break;
				}

				page.Items[javascriptUrl] = new object();
			}
		}

		public static void RegisterJavascriptResource(this ClientScriptManager clientScriptManager, Type type, string resourceName)
		{
			RegisterJavascriptResource(clientScriptManager, type, resourceName, ResourceInsertPosition.HeaderBottom);
		}

		public static void RegisterJavascriptResource(this ClientScriptManager clientScriptManager, Type type, string resourceName, string prefix, string postfix)
		{
			RegisterJavascriptResource(clientScriptManager, type, resourceName, ResourceInsertPosition.HeaderBottom, prefix, postfix);
		}

		public static void RegisterJavascriptResource(this ClientScriptManager clientScriptManager, Type type, string resourceName, ResourceInsertPosition position)
		{
			RegisterJavascriptResource(clientScriptManager, type, resourceName, position, string.Empty, string.Empty);
		}

		public static void RegisterJavascriptResource(this ClientScriptManager clientScriptManager, Type type, string resourceName, ResourceInsertPosition position, string prefix, string postfix)
		{
			string javascriptUrl = clientScriptManager.GetWebResourceUrl(type, resourceName);
			RegisterJavascriptInclude(clientScriptManager, javascriptUrl, position, prefix, postfix);
		}

		public static void RegisterEmbeddedJavascriptResource(this ClientScriptManager clientScriptManager, Type type, string resourceName, ResourceInsertPosition position)
		{
			string javascriptUrl = EmbeddedWebResourceUtility.GetUrl(type.Assembly, resourceName);
			RegisterJavascriptInclude(clientScriptManager, javascriptUrl, position);
		}

		public static void RegisterEmbeddedJavascriptResource(this ClientScriptManager clientScriptManager, Type type, string resourceName)
		{
			RegisterEmbeddedJavascriptResource(clientScriptManager, type, resourceName, ResourceInsertPosition.HeaderBottom);
		}
	}

	public enum ResourceInsertPosition
	{
		HeaderTop,
		HeaderBottom,
		BodyBottom
	}
}