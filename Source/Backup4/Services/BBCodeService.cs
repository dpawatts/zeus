using CodeKicker.BBCode;

namespace Zeus.Templates.Services
{
	public class BBCodeService
	{
		public string GetHtml(string bbCode)
		{
			var parser = new BBCodeParser(
				new []
					{
						new BBTag("b", "<b>", "</b>"),
						new BBTag("i", "<span style=\"font-style:italic;\">", "</span>"),
						new BBTag("u", "<span style=\"text-decoration:underline;\">", "</span>"),
						new BBTag("img", "<img src=\"${content}\" />", string.Empty, false, true),
						new BBTag("url", "<a href=\"${href}\">", "</a>", new BBAttribute("href", string.Empty), new BBAttribute("href", "href")),
						new BBTag("size", "<span style=\"font-size:${size}%;\">", "</span>", new BBAttribute("size", string.Empty), new BBAttribute("size", "size")),
						new BBTag("list", "<ul>", "</ul>"),
						new BBTag("*", "<li>", "</li>", true, false),
						new BBTag("quote", "<blockquote>", "</blockquote>"),
						new BBTag("code", "<pre>", "</pre>")
					});
			try
			{
				return parser.ToHtml(bbCode).Replace("\r", string.Empty).Replace("\n", "<br />");
			}
			catch (BBCodeParsingException ex)
			{
				return "Error: " + ex.Message;
			}
		}
	}
}