using System;
using System.Linq;
using System.Web.Mvc;
using Zeus.Linq;
using Zeus.Templates.ContentTypes;
using Zeus.Web;
using Zeus.Web.UI.WebControls;

namespace Zeus.Templates.Mvc.Html
{
	public static class NavigationExtensions
	{
		public static string NavigationLinks(this HtmlHelper html, string prefix, string postfix, string activePageCssClass, ContentItem currentPage)
		{
			string result = prefix;
			foreach (ContentItem contentItem in Find.StartPage.GetGlobalizedChildren().Navigable())
				result += string.Format("<li {1}><a href=\"{0}\">{2}</a></li>",
					contentItem.Url,
					(((contentItem is Redirect) && ((Redirect)contentItem).RedirectItem == currentPage) || Find.IsAccessibleChildOrSelf(contentItem, currentPage)) ? " class=\"" + activePageCssClass + "\"" : string.Empty,
					contentItem.Title);
			result += postfix;
			return result;
		}

		public static string NavigationLinks(this HtmlHelper html, ContentItem currentPage)
		{
			return NavigationLinks(html, "<ul>", "</ul>", "on", currentPage);
		}

		public static string Breadcrumbs(this HtmlHelper html, ContentItem currentPage)
		{
			return Breadcrumbs(html, currentPage, "<ul id=\"crumbs\">", "</ul>", 1, 2, string.Empty,
				l => string.Format("<li><a href=\"{0}\">{1}</a></li>", l.Url, l.Contents),
				l => string.Format("<li class=\"last\"><a href=\"{0}\">{1}</a></li>", l.Url, l.Contents));
		}

		public static string Breadcrumbs(this HtmlHelper html, ContentItem currentPage, string prefix, string postfix, int startLevel, int visibilityLevel, string separatorText,
			Func<ILink, string> itemCallback, Func<ILink, string> lastItemCallback)
		{
			string result = postfix;

			int added = 0;
			var parents = Find.EnumerateParents(currentPage, Find.StartPage, true);
			if (startLevel != 1 && parents.Count() >= startLevel)
				parents = parents.Take(parents.Count() - startLevel);
			foreach (ContentItem page in parents)
			{
				IBreadcrumbAppearance appearance = page as IBreadcrumbAppearance;
				bool visible = appearance == null || appearance.VisibleInBreadcrumb;
				if (visible && page.IsPage)
				{
					ILink link = appearance ?? (ILink)page;
					if (added > 0)
					{
						result = separatorText + Environment.NewLine + result;
						result = GetBreadcrumbItem(link, itemCallback) + result;
					}
					else
					{
						result = GetBreadcrumbItem(link, lastItemCallback) + result;
					}
					result = Environment.NewLine + result;
					++added;
				}
			}

			result = prefix + result;

			if (added < visibilityLevel)
				result = string.Empty;

			return result;
		}

		private static string GetBreadcrumbItem(ILink link, Func<ILink, string> formatCallback)
		{
			return formatCallback(link);
		}
	}
}