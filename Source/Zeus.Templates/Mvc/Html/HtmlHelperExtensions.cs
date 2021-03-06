using System.Web.Mvc;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Html
{
	public static class HtmlHelperExtensions
	{
		public static string FormLabel(this HtmlHelper html, string @for, string text, bool required)
		{
			string format = @"<label for=""{0}"">{1}";
			if (required)
				format += "<span>*</span>";
			format += "</label>";

			return string.Format(format, @for.Replace(".", "_"), text);
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
            if (contentItem is PageContentItem)
            {
                PageContentItem thePage = contentItem as PageContentItem;
                if (thePage.UseProgrammableSEOAssets)
                    return thePage.ProgrammableHtmlTitle;
            }
            return html.PropertyOrDefault(contentItem, SeoUtility.HTML_TITLE, "Title");        
		}

		public static string MetaKeywords(this HtmlHelper html, ContentItem contentItem)
		{
            if (contentItem is PageContentItem)
            {
                PageContentItem thePage = contentItem as PageContentItem;
                if (thePage.UseProgrammableSEOAssets)
                    return thePage.ProgrammableMetaKeywords;
            }
			return html.PropertyOrDefault(contentItem, SeoUtility.META_KEYWORDS, "Title");
		}

		public static string MetaDescription(this HtmlHelper html, ContentItem contentItem)
		{
            if (contentItem is PageContentItem)
            {
                PageContentItem thePage = contentItem as PageContentItem;
                if (thePage.UseProgrammableSEOAssets)
                    return thePage.ProgrammableMetaDescription;
            }
			return html.PropertyOrDefault(contentItem, SeoUtility.META_DESCRIPTION, "Title");
		}

		public static string Pluralize(this HtmlHelper html, string word, int count)
		{
			if (count != 1)
				word = word + "s";
			return word;
		}

		public static string Pluralize(this HtmlHelper html, string zeroItems, string oneItem, string multipleItems, int count)
		{
			if (count < 1)
				return zeroItems;
			if (count == 1)
				return oneItem;
			return string.Format(multipleItems, count);
		}

		public static string IsAre(this HtmlHelper html, int count)
		{
			if (count != 1)
				return "are";
			return "is";
		}
	}
}