using System.Web.Mvc;

namespace Zeus.Templates.Mvc.Html
{
	public static class HtmlHelperExtensions
	{
		public static string Label(this HtmlHelper html, string @for, string text, bool required)
		{
			string format = @"<label for=""{0}"">{1}";
			if (required)
				format += "<span>*</span>";
			format += "</label>";

			return string.Format(format, @for.Replace(".", "_"), text);
		}

		public static string Label(this HtmlHelper html, string @for, string text)
		{
			return Label(html, @for, text, false);
		}

		public static string PropertyOrDefault(this HtmlHelper html, ContentItem contentItem, string propertyName, string fallbackPropertyName)
		{
			if (contentItem == null)
				return string.Empty;

			if (!contentItem.Details.ContainsKey(propertyName))
				return contentItem[fallbackPropertyName].ToString();

			return contentItem[propertyName].ToString();
		}

		public static string HtmlTitle(this HtmlHelper html, ContentItem contentItem)
		{
			return html.PropertyOrDefault(contentItem, SeoUtility.HTML_TITLE, "Title");
		}

		public static string MetaKeywords(this HtmlHelper html, ContentItem contentItem)
		{
			return html.PropertyOrDefault(contentItem, SeoUtility.META_KEYWORDS, "Title");
		}

		public static string MetaDescription(this HtmlHelper html, ContentItem contentItem)
		{
			return html.PropertyOrDefault(contentItem, SeoUtility.META_DESCRIPTION, "Title");
		}

		public static string Pluralize(this HtmlHelper html, string word, int count)
		{
			if (count != 1)
				word = word + "s";
			return word;
		}

		public static string IsAre(this HtmlHelper html, int count)
		{
			if (count != 1)
				return "are";
			return "is";
		}
	}
}