using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Zeus.Templates.Mvc.Html
{
	public static class ValidationExtensions
	{
		public static string ValidationSummaryBox(this HtmlHelper htmlHelper, string message)
		{
			if (htmlHelper.ViewData.ModelState.IsValid)
				return null;

			TagBuilder divBuilder = new TagBuilder("div");
			divBuilder.Attributes["id"] = "errors";

			if (!string.IsNullOrEmpty(message))
			{
				TagBuilder messageBuilder = new TagBuilder("p");
				messageBuilder.SetInnerText(message);
				divBuilder.InnerHtml += messageBuilder.ToString(TagRenderMode.Normal) + "<br /><br />" + Environment.NewLine;
			}

			StringBuilder builder = new StringBuilder();
			TagBuilder listBuilder = new TagBuilder("ul");
			foreach (ModelState state in htmlHelper.ViewData.ModelState.Values)
			{
				foreach (ModelError error in state.Errors)
				{
					string errorMessage = GetUserErrorMessageOrDefault(htmlHelper.ViewContext.HttpContext, error, null);
					if (!string.IsNullOrEmpty(errorMessage))
					{
						TagBuilder liBuilder = new TagBuilder("li");
						liBuilder.SetInnerText(errorMessage);
						builder.AppendLine(liBuilder.ToString(TagRenderMode.Normal));
					}
				}
			}
			listBuilder.InnerHtml = builder.ToString();
			divBuilder.InnerHtml += listBuilder.ToString(TagRenderMode.Normal);
			return divBuilder.ToString(TagRenderMode.Normal);
		}

		private static string GetUserErrorMessageOrDefault(HttpContextBase httpContext, ModelError error, ModelState modelState)
		{
			if (!string.IsNullOrEmpty(error.ErrorMessage))
				return error.ErrorMessage;

			if (modelState == null)
				return null;

			string value = (modelState.Value != null) ? modelState.Value.AttemptedValue : null;
			return string.Format(CultureInfo.CurrentCulture, "The value '{0}' is invalid.", value);
		}
	}
}