using System;
using System.Text.RegularExpressions;

namespace Zeus.Web
{
	public class PermanentLinkManager : IPermanentLinkManager
	{
		public string ResolvePermanentLinks(string value)
		{
			const string pattern = @"href=""~/link/([\d]+?)""";
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
			string stage1 = regex.Replace(value, OnPatternMatched);
			
			const string pattern2 = @"href=""/~/link/([\d]+?)""";
			Regex regex2 = new Regex(pattern2, RegexOptions.IgnoreCase);

			return regex2.Replace(stage1, OnPatternMatched);
		}

		private string OnPatternMatched(Match match)
		{
			// Get ContentID from link.
			int contentID = Convert.ToInt32(match.Groups[1].Value);

			// Load content item and get URL.
			ContentItem contentItem = Context.Persister.Get(contentID);
			return string.Format(@"href=""{0}""", (contentItem != null) ? contentItem.Url : "#");
		}
	}
}