using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Zeus.Linq;
using Zeus.Templates.ContentTypes;
using Zeus.Web;
using Zeus.Web.UI.WebControls;

namespace Zeus.Templates.Mvc.Html
{
	public static class NavigationExtensions
	{
		public delegate string CssClassFunc(ContentItem contentItem, bool isFirst, bool isLast);

		public static IEnumerable<ContentItem> NavigationPages(this HtmlHelper html)
		{
			return NavigationPages(html, Find.StartPage);
		}

		public static IEnumerable<ContentItem> NavigationPages(this HtmlHelper html, ContentItem startPage)
		{
			return startPage.GetGlobalizedChildren().NavigablePages();
		}

		public static string NavigationLinks(this HtmlHelper html, ContentItem startItem, Func<string, string> layoutCallback,
			CssClassFunc cssClassCallback)
		{
			var navigationItems = NavigationPages(html, startItem);

			string result = string.Empty;
			foreach (ContentItem contentItem in navigationItems)
				result += string.Format("<li class=\"{0}\"><span><a href=\"{1}\">{2}</a></span></li>",
					cssClassCallback(contentItem, contentItem == navigationItems.First(), contentItem == navigationItems.Last()),
					contentItem.Url, contentItem.Title);

			result = layoutCallback(result);
			return result;
		}

		public static string NavigationLinks(this HtmlHelper html, ContentItem currentPage)
		{
			return NavigationLinks(html,
				Find.StartPage,
				nl => "<ul>" + nl + "</ul>",
				(ci, isFirst, isLast) =>
				{
					string result = string.Empty;
					if (IsCurrentBranch(html, ci, currentPage))
						result += "on";
					if (isLast)
						result += " last";
					return result;
				});
		}

		public static string NavigationLinks(this HtmlHelper html, ContentItem startItem, ContentItem currentPage, string listClientId)
		{
			return NavigationLinks(html,
				startItem,
				nl => "<ul id=\"" + listClientId + "\">" + nl + "</ul>",
				(ci, isFirst, isLast) =>
				{
					string result = string.Empty;
					if (IsCurrentBranch(html, ci, currentPage))
						result += "on";
					if (isLast)
						result += " last";
					return result;
				});
		}

		/// <summary>
		/// Returns true if this page, or one of its descendents, is the current page.
		/// </summary>
		/// <param name="helper"></param>
		/// <param name="itemToCheck"></param>
		/// <returns></returns>
		public static bool IsCurrentBranch(this HtmlHelper helper, ContentItem itemToCheck)
		{
			return IsCurrentBranch(helper, itemToCheck, Find.CurrentPage);
		}

		public static bool IsCurrentBranch(this HtmlHelper helper, ContentItem itemToCheck, ContentItem currentPage)
		{
			if ((itemToCheck is Redirect))
			{
				Redirect redirect = (Redirect) itemToCheck;
				if (redirect.RedirectItem == currentPage)
					return true;
				if (redirect.CheckChildrenForNavigationState && Find.IsAccessibleChildOrSelf(((Redirect)itemToCheck).RedirectItem, currentPage))
					return true;
			}
			if (Find.IsAccessibleChildOrSelf(itemToCheck, currentPage))
				return true;
			return false;
		}

		public static string Breadcrumbs(this HtmlHelper html, ContentItem currentPage)
		{
			return Breadcrumbs(html, currentPage, "<ul id=\"crumbs\">", "</ul>", 1, 2, string.Empty,
				l => string.Format("<li><a href=\"{0}\">{1}</a></li>", l.Url, l.Contents),
				l => string.Format("<li class=\"last\">{0}</li>", l.Contents));
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

		public static string Sitemap(this HtmlHelper html)
		{
			StringBuilder sb = new StringBuilder();
			foreach (ContentItem contentItem in Find.StartPage.GetChildren().Pages().Visible())
			{
				sb.AppendFormat("<h4><a href=\"{0}\">{1}</a></h4>", contentItem.Url, contentItem.Title);
				SitemapRecursive(contentItem, sb);
			}
			return sb.ToString();
		}

		private static bool SitemapRecursive(ContentItem contentItem, StringBuilder sb)
		{
			var childItems = contentItem.GetChildren().Visible();

			bool foundSomething = false;

			StringBuilder sbInner = new StringBuilder();

			if (childItems.Any())
			{
				sbInner.Append("<ul>");
				foreach (ContentItem childItem in childItems)
				{
					ISitemapAppearance appearance = childItem as ISitemapAppearance;
					//the appearance != null bit means that by default items won't show as links
					bool visible = childItem.Visible && (childItem.IsPage || (appearance != null && appearance.VisibleInSitemap));
					
					if (visible)
					{
						sbInner.Append("<li>");

						sbInner.AppendFormat("<a href=\"{0}\">{1}</a>", childItem.Url, childItem.Title);
						foundSomething = true;

						//something has been found on this tree path, but still possibility of dead ends, so start again!
						StringBuilder sbInnerFurther = new StringBuilder();
						SitemapRecursive(childItem, sbInnerFurther);
						sbInner.Append(sbInnerFurther.ToString());

						sbInner.Append("</li>");
					}
					else
					{
						//nothing found yet, so don't add anything but keep checking - progress so far needs to be saved to the new stringbuilder
						StringBuilder sbInnerFurther = new StringBuilder();
						sbInnerFurther.Append("<li>");
						sbInnerFurther.Append(childItem.Title);

						//this will return if something is found further down the tree
						foundSomething = SitemapRecursive(childItem, sbInnerFurther);

						//only add this level if something was found!
						if (foundSomething)
							sbInner.Append(sbInnerFurther.ToString() + "</li>");
					}
				}
				sbInner.Append("</ul>");
			}

			//only append to the final string, if at somepoint a link has been hit, otherwise, you'll end up with loads of titles for no reason
			if (foundSomething)
				sb.Append(sbInner.ToString());

			return foundSomething;
		}
	}
}