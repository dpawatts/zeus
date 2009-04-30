using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Zeus.AddIns.Forums
{
	public static class BBCodeHelper
	{
		private static readonly List<Smiley> _smilies;

		static BBCodeHelper()
		{
			_smilies = new List<Smiley>();
			AddSmiley(":D", "icon_e_biggrin.gif", 15, 17, "Very Happy");
			AddSmiley(":)", "icon_e_smile.gif", 15, 17, "Smile");
			AddSmiley(";)", "icon_e_wink.gif", 15, 17, "Wink");
			AddSmiley(":(", "icon_e_sad.gif", 15, 17, "Sad");
			AddSmiley(":o", "icon_e_surprised.gif", 15, 17, "Surprised");
			AddSmiley(":shock:", "icon_eek.gif", 15, 17, "Shocked");
			AddSmiley(":?", "icon_e_confused.gif", 15, 17, "Confused");
			AddSmiley("8-)", "icon_cool.gif", 15, 17, "Cool");
			AddSmiley(":lol:", "icon_lol.gif", 15, 17, "Laughing");
			AddSmiley(":x", "icon_mad.gif", 15, 17, "Mad");
			AddSmiley(":P", "icon_razz.gif", 15, 17, "Razz");
			AddSmiley(":oops:", "icon_redface.gif", 15, 17, "Embarrassed");
			AddSmiley(":cry:", "icon_cry.gif", 15, 17, "Crying or Very Sad");
			AddSmiley(":evil:", "icon_evil.gif", 15, 17, "Evil or Very Mad");
			AddSmiley(":twisted:", "icon_twisted.gif", 15, 17, "Twisted Evil");
			AddSmiley(":roll:", "icon_rolleyes.gif", 15, 17, "Rolling Eyes");
			AddSmiley(":!:", "icon_exclaim.gif", 15, 17, "Exclamation");
			AddSmiley(":?:", "icon_question.gif", 15, 17, "Question");
			AddSmiley(":idea:", "icon_idea.gif", 15, 17, "Idea");
			AddSmiley(":arrow:", "icon_arrow.gif", 15, 17, "Arrow");
			AddSmiley(":|", "icon_neutral.gif", 15, 17, "Neutral");
			AddSmiley(":mrgreen:", "icon_mrgreen.gif", 15, 17, "Mr. Green");
			AddSmiley(":geek:", "icon_e_geek.gif", 17, 17, "Geek");
			AddSmiley(":ugeek:", "icon_e_ugeek.gif", 17, 18, "Uber Geek");
		}

		private static void AddSmiley(string text, string imageName, int width, int height, string title)
		{
			_smilies.Add(new Smiley
			{
				Text = text,
				ImageUrl = new Page().ClientScript.GetWebResourceUrl(typeof(BBCodeHelper), "Zeus.AddIns.Forums.Web.Resources.Smilies." + imageName),
				Width = width,
				Height = height,
				Title = title
			});
		}

		public static IEnumerable<Smiley> Smilies
		{
			get { return _smilies; }
		}

		/// <summary>
		/// Converts the input plain-text BBCode to HTML output and replacing carriage returns
		/// and spaces with <br /> and   etc...
		/// Recommended: Use this function only during storage and updates.
		/// Keep a seperate field in your database for HTML formatted content and raw text.
		/// An optional third, plain text field, with no formatting info will make full text searching
		/// more accurate.
		/// E.G. BodyText(with BBCode for textarea/WYSIWYG), BodyPlain(plain text for searching),
		/// BodyHtml(formatted HTML for output pages)
		/// </summary>
		public static string ConvertToHtml(string content)
		{
			// Clean your content here... E.G.:
			// content = CleanText(content);

			// Basic tag stripping for this example (PLEASE EXTEND THIS!)
			content = StripTags(content);

			// Smilies
			foreach (Smiley smiley in Smilies)
			{
				string imageTag = string.Format("<img src=\"{0}\" width=\"{1}\" height=\"{2}\" alt=\"{3}\" title=\"{4}\" />",
					smiley.ImageUrl, smiley.Width, smiley.Height, smiley.Text, smiley.Title);
				string pattern = smiley.Text.Replace(")", "\\)").Replace("(", "\\(").Replace("|", "\\|").Replace("?", "\\?");
				content = MatchReplace(pattern, imageTag, content);
			}

			content = MatchReplace(@"\[b\]([^]]+)\[/b\]", "<strong>$1</strong>", content);
			content = MatchReplace(@"\[i\]([^]]+)\[/i\]", "<em>$1</em>", content);
			content = MatchReplace(@"\[u\]([^]]+)\[/u\]", "<span style=\"text-decoration:underline\">$1</span>", content);
			content = MatchReplace(@"\[del\]([^]]+)\[/del\]", "<span style=\"text-decoration:line-through\">$1</span>", content);

			// Colors and sizes
			content = MatchReplace(@"\[color=(#[0-9a-fA-F]{6}|[a-z-]+)\]([^]]+)\[/color\]", "<span style=\"color:$1;\">$2</span>", content);
			content = MatchReplace(@"\[size=([0-9]+)\]([^\]]+)\[/size\]", "<span style=\"font-size:$1%; font-weight:normal;\">$2</span>", content);

			// Text alignment
			content = MatchReplace(@"\[left\]([^]]+)\[/left\]", "<span style=\"text-align:left\">$1</span>", content);
			content = MatchReplace(@"\[right\]([^]]+)\[/right\]", "<span style=\"text-align:right\">$1</span>", content);
			content = MatchReplace(@"\[center\]([^]]+)\[/center\]", "<span style=\"text-align:center\">$1</span>", content);
			content = MatchReplace(@"\[justify\]([^]]+)\[/justify\]", "<span style=\"text-align:justify\">$1</span>", content);

			// HTML Links
			content = MatchReplace(@"\[url\]([^]]+)\[/url\]", "<a href=\"$1\">$1</a>", content);
			content = MatchReplace(@"\[url=([^]]+)\]([^]]+)\[/url\]", "<a href=\"$1\">$2</a>", content);

			// Images
			content = MatchReplace(@"\[img\]([^]]+)\[/img\]", "<img src=\"$1\" alt=\"\" />", content);
			content = MatchReplace(@"\[img=([^]]+)\]([^]]+)\[/img\]", "<img src=\"$2\" alt=\"$1\" />", content);

			// Lists
			content = MatchReplace(@"\[\*\]([^[]+)", "<li>$1</li>", content);
			content = MatchReplace(@"\[list\]([^]]+)\[/list\]", "</p><ul>$1</ul><p>", content);
			content = MatchReplace(@"\[list=1\]([^]]+)\[/list\]", "</p><ol>$1</ol><p>", content);

			// Headers
			content = MatchReplace(@"\[h1\]([^]]+)\[/h1\]", "<h1>$1</h1>", content);
			content = MatchReplace(@"\[h2\]([^]]+)\[/h2\]", "<h2>$1</h2>", content);
			content = MatchReplace(@"\[h3\]([^]]+)\[/h3\]", "<h3>$1</h3>", content);
			content = MatchReplace(@"\[h4\]([^]]+)\[/h4\]", "<h4>$1</h4>", content);
			content = MatchReplace(@"\[h5\]([^]]+)\[/h5\]", "<h5>$1</h5>", content);
			content = MatchReplace(@"\[h6\]([^]]+)\[/h6\]", "<h6>$1</h6>", content);

			// Horizontal rule
			content = MatchReplace(@"\[hr\]", "<hr />", content);

			// Set a maximum quote depth (In this case, hard coded to 3)
			for (int i = 1; i < 3; i++)
			{
				// Quotes
				content = MatchReplace(@"\[quote=([^]]+)@([^]]+)\]([^]]+)\[/quote\]", "</p><div class=\"block\"><blockquote><cite>$1 wrote on $2</cite><hr /><p>$3</p></blockquote></div><p>", content);
				content = MatchReplace(@"\[quote=([^]]+)\]([^]]+)\[/quote\]", "</p><div class=\"block\"><blockquote><cite>$1 wrote</cite><hr /><p>$2</p></blockquote></div><p>", content);
				content = MatchReplace(@"\[quote\]([^]]+)\[/quote\]", "</p><div class=\"block\"><blockquote><p>$1</p></blockquote></div><p>", content);
			}

			// The following markup is for embedded video -->

			// YouTube
			content = MatchReplace(@"\[youtube\]http://([a-zA-Z]+.)youtube.com/watch?v=([a-zA-Z0-9_-]+)\[/youtube\]",
				"<object width=\"425\" height=\"344\"><param name=\"movie\" value=\"http://www.youtube.com/v/$2\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed src=\"http://www.youtube.com/v/$2\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"425\" height=\"344\"></embed></object>", content);

			// LiveVideo
			content = MatchReplace(@"\[livevideo\]http://([a-zA-Z]+.)livevideo.com/video/([a-zA-Z0-9_-]+)/([a-zA-Z0-9]+)/([a-zA-Z0-9_-]+).aspx\[/livevideo\]",
				"<object width=\"445\" height=\"369\"><embed src=\"http://www.livevideo.com/flvplayer/embed/$3\" type=\"application/x-shockwave-flash\" quality=\"high\" width=\"445\" height=\"369\" wmode=\"transparent\"></embed></object>", content);

			// LiveVideo (There are two types of links for LV)
			content = MatchReplace(@"\[livevideo\]http://([a-zA-Z]+.)livevideo.com/video/([a-zA-Z0-9]+)/([a-zA-Z0-9_-]+).aspx\[/livevideo\]",
				"<object width=\"445\" height=\"369\"><embed src=\"http://www.livevideo.com/flvplayer/embed/$2\" type=\"application/x-shockwave-flash\" quality=\"high\" width=\"445\" height=\"369\" wmode=\"transparent\"></embed></object>", content);

			// Metacafe
			content = MatchReplace(@"\[metacafe\]http://([a-zA-Z]+.)metacafe.com/watch/([0-9]+)/([a-zA-Z0-9_]+)/\[/metacafe\]",
				"<object width=\"400\" height=\"345\"><embed src=\"http://www.metacafe.com/fplayer/$2/$3.swf\" width=\"400\" height=\"345\" wmode=\"transparent\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\"></embed></object>", content);

			// LiveLeak
			content = MatchReplace(@"\[liveleak\]http://([a-zA-Z]+.)liveleak.com/view?i=([a-zA-Z0-9_]+)\[/liveleak\]",
				"<object width=\"450\" height=\"370\"><param name=\"movie\" value=\"http://www.liveleak.com/e/$2\"></param><param name=\"wmode\" value=\"transparent\"></param><embed src=\"http://www.liveleak.com/e/59a_1231807882\" type=\"application/x-shockwave-flash\" wmode=\"transparent\" width=\"450\" height=\"370\"></embed></object>", content);

			// <-- End video markup

			// Google and Wikipedia page links
			content = MatchReplace(@"\[google\]([^]]+)\[/google\]", "<a href=\"http://www.google.com/search?q=$1\">$1</a>", content);
			content = MatchReplace(@"\[wikipedia\]([^]]+)\[/wikipedia\]", "<a href=\"http://www.wikipedia.org/wiki/$1\">$1</a>", content);

			// Put the content in a paragraph
			content = "<p>" + content + "</p>";

			// Clean up a few potential markup problems
			content = content.Replace(@"\t", "&nbsp;&nbsp;&nbsp;&nbsp;")
				.Replace("  ", "&nbsp;&nbsp;")
				.Replace("<br /></p>", "</p>")
				.Replace("<p><br />", "<p>")
				.Replace("<p><blockquote>", "<blockquote>")
				.Replace("</blockquote></p>", "</blockquote>")
				.Replace("<p></p>", "")
				.Replace("<p><ul></p>", "<ul>")
				.Replace("<p></ul></p>", "</ul>")
				.Replace("<p><ol></p>", "<ol>")
				.Replace("<p></ol></p>", "</ol>")
				.Replace("<p><li>", "<li><p>")
				.Replace("</li></p>", "</p></li>")
				.Replace(Environment.NewLine, "<br />");

			return content;
		}

		/// <summary>
		/// Strip any existing HTML tags
		/// </summary>
		/// <param name="content">Raw input from user</param>
		/// <returns>Tag stripped storage safe text</returns>
		public static string StripTags(string content)
		{
			return MatchReplace(@"<[^>]+>", "", content, true, true, true);
		}

		public static string MatchReplace(string pattern, string match, string content)
		{
			return MatchReplace(pattern, match, content, false, false, false);
		}

		public static string MatchReplace(string pattern, string match, string content, bool multi)
		{
			return MatchReplace(pattern, match, content, multi, false, false);
		}

		public static string MatchReplace(string pattern, string match, string content, bool multi, bool white)
		{
			return MatchReplace(pattern, match, content, multi, white);
		}

		/// <summary>
		/// Match and replace a specific pattern with formatted text
		/// </summary>
		/// <param name="pattern">Regular expression pattern</param>
		/// <param name="match">Match replacement</param>
		/// <param name="content">Text to format</param>
		/// <param name="multi">Multiline text (optional)</param>
		/// <param name="white">Ignore white space (optional)</param>
		/// <returns>HTML Formatted from the original BBCode</returns>
		private static string MatchReplace(string pattern, string match, string content, bool multi, bool white, bool cult)
		{
			if (multi && white && cult)
				return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
			else if (multi && white)
				return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnoreCase);
			else if (multi && cult)
				return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
			else if (white && cult)
				return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);
			else if (multi)
				return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline);
			else if (white)
				return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
			else if (cult)
				return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

			// Default
			return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase);
		}

		public class Smiley
		{
			public string Text { get; internal set; }
			public string ImageUrl { get; internal set; }
			public int Width { get; internal set; }
			public int Height { get; internal set; }
			public string Title { get; internal set; }
		}
	}
}