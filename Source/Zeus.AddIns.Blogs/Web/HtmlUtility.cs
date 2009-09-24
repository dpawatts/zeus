using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Zeus.AddIns.Blogs.Web
{
	public static class HtmlUtility
	{
		/// <summary>
		/// Returns a string collection of URLs within the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static IEnumerable<string> GetLinks(string text)
		{
			var links = new List<string>();
			const string pattern = @"(?:[hH][rR][eE][fF]\s*=)" +
			                       @"(?:[\s""']*)(?!#|[Mm]ailto|[lL]ocation.|[jJ]avascript|.*css|.*this\.)" +
			                       @"(.*?)(?:[\s>""'])";
			var r = new Regex(pattern, RegexOptions.IgnoreCase);
			for (Match m = r.Match(text); m.Success; m = m.NextMatch())
				if (m.Groups.ToString().Length > 0)
				{
					string link = m.Groups[1].ToString();
					if (!links.Contains(link))
						links.Add(link);
				}
			return links;
		}
	}
}