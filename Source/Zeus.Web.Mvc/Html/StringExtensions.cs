using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Zeus.BaseLibrary.ExtensionMethods;

namespace Zeus.Web.Mvc.Html
{
	public static class StringExtensions
	{
		/// <summary>
		/// Wraps an anchor tag around all urls. Makes sure not to wrap already
		/// wrapped urls.
		/// </summary>
		/// <param name="html">Html containing urls to convert.</param>
		/// <returns></returns>
		public static string ConvertUrlsToHyperLinks(this HtmlHelper htmlHelper, string html)
		{
			if (html == null)
				throw new ArgumentNullException("html");

			if (html.Length == 0)
				return string.Empty;

			const string pattern = @"((https?|ftp)://|www\.)[\w]+(.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])";
			MatchCollection matches = Regex.Matches(html, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			foreach (Match m in matches)
			{
				string httpPortion = string.Empty;
				if (!m.Value.Contains("://"))
				{
					httpPortion = "http://";
				}

				html = html.Replace(m.Value,
					string.Format(CultureInfo.InvariantCulture,
						"<a rel=\"nofollow external\" href=\"{0}{1}\" title=\"{1}\">{2}</a>",
						httpPortion, m.Value, ShortenUrl(htmlHelper, m.Value, 50))
					);
			}
			return html;
		}

		/// <summary>
		/// Shortens a url for display.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="max">Maximum size for the url. Anything longer gets shortened.</param>
		/// <returns></returns>
		public static string ShortenUrl(this HtmlHelper htmlHelper, string url, int max)
		{
			if (url == null)
				throw new ArgumentNullException("url");

			if (max < 5)
				throw new ArgumentException("We will not shorten a URL to less than 5 characters. Come on now!", "max");

			if (url.Length <= max)
				return url;

			// Remove the protocal
			url = url.RightAfter("://");

			if (url.Length <= max)
				return url;

			// Remove the folder structure, except for the last folder.
			int firstIndex = url.IndexOf("/") + 1;
			int startIndexForLastSlash = url.Length - 1;
			if (url.EndsWith("/"))
				startIndexForLastSlash--;

			int lastIndex = url.LastIndexOf("/", startIndexForLastSlash);

			if (firstIndex < lastIndex)
				url = url.LeftBefore("/") + "/.../" + url.RightAfterLast("/", startIndexForLastSlash, StringComparison.Ordinal);

			if (url.Length <= max)
				return url;

			// Remove URL parameters
			url = url.LeftBefore("?");

			if (url.Length <= max)
				return url;

			// Remove URL fragment
			url = url.LeftBefore("#");

			if (url.Length <= max)
				return url;

			// Shorten page
			firstIndex = url.LastIndexOf("/") + 1;
			lastIndex = url.LastIndexOf(".");
			if (lastIndex - firstIndex > 10)
			{
				string page = url.Substring(firstIndex, lastIndex - firstIndex);
				int length = url.Length - max + 3;
				url = url.Replace(page, "..." + page.Substring(length));
			}

			if (url.Length <= max)
				return url;

			//Trim of trailing slash if any.
			if (url.Length > max && url.EndsWith("/"))
				url = url.Substring(0, url.Length - 1);

			if (url.Length <= max)
				return url;

			if (url.Length > max)
				url = url.Substring(0, max - 3) + "...";

			return url;
		}
	}
}