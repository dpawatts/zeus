using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Zeus.Web.Mvc.Html
{
	public static class ValidationExtensions
	{
		public static bool HasModelError(this HtmlHelper htmlHelper, string modelName)
		{
			return !MvcHtmlString.IsNullOrEmpty(htmlHelper.ValidationMessage(modelName));
		}
	}
}