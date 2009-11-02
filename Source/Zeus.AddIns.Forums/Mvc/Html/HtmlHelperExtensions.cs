using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Zeus.BaseLibrary.ExtensionMethods.Text;

namespace Zeus.AddIns.Forums.Mvc.Html
{
	public static class HtmlHelperExtensions
	{
		public static string BBCodeTextBox(this HtmlHelper html, string name)
		{
			StringBuilder sb = new StringBuilder();

			// Colour palette.
			sb.AppendLine(@"<div id=""colourPalette"" style=""display:none"">");
			sb.AppendLine(@"<dl style=""clear:left""><dt><p>Font colour:</p></dt>");
			sb.AppendLine(@"<script type=""text/javascript"">");
			sb.AppendLine(@"function change_palette(link) {");
			sb.AppendLine(@"$('#colourPalette').toggle();");
			sb.AppendLine(@"link.value = ($('#colourPalette').css('display') == 'block') ? 'Hide font colour' : 'Font colour';");
			sb.AppendLine(@"}");
			sb.AppendLine(@"colorPalette('h', 15, 10);");
			sb.AppendLine(@"</script></dd></dl></div>");

			sb.AppendLine(@"<div>");

			// Buttons
			AddButton(sb, "b", " B ", "font-weight:bold; width: 30px", "bbstyle(0)", "Bold text: [b]text[/b]");
			AddButton(sb, "i", " i ", "font-style:italic; width: 30px", "bbstyle(2)", "Italic text: [i]text[/i]");
			AddButton(sb, "u", " u ", "text-decoration:underline; width: 30px", "bbstyle(4)", "Underline text: [u]text[/u]");
			AddButton(sb, "q", "Quote", "width: 50px", "bbstyle(6)", "Quote text: [quote]text[/quote]");
			AddButton(sb, "c", "Code", "width: 40px", "bbstyle(8)", "Code display: [code]text[/code]");
			AddButton(sb, "l", "List", "width: 40px", "bbstyle(10)", "List: [list]text[/list]");
			AddButton(sb, "o", "List=", "width: 40px", "bbstyle(12)", "Ordered list: [list=]text[/list]");
			AddButton(sb, "t", "[*]", "width: 40px", "bbstyle(-1)", "List item: [*]text[/*]");
			AddButton(sb, "p", "Img", "width: 40px", "bbstyle(14)", "Insert image: [img]http://image_url[/img]");
			AddButton(sb, "w", "URL", "text-decoration:underline; width: 40px", "bbstyle(16)", "Insert URL: [url]http://url[/url] or [url=http://url]URL text[/url]");
			AddButton(sb, "d", "Flash", string.Empty, "bbstyle(18)", "Flash: [flash=width,height]http://url[/flash]");

			// Font size select
			var fontSizeOptions = new []
			{
				new SelectListItem { Text = "Tiny", Value = "50" },
				new SelectListItem { Text = "Small", Value = "85" },
				new SelectListItem { Text = "Normal", Value = "100", Selected = true },
				new SelectListItem { Text = "Large", Value = "150" },
				new SelectListItem { Text = "Huge", Value = "200" },
			};
			sb.Append(html.DropDownList("fontSize", fontSizeOptions, new { onchange = "bbfontstyle('[size=' + $(this).val() + ']', '[/size]'); $(this).val('100');", title = "Font size: [size=85]small text[/size]" }));

			// Font colour
			AddButton(sb, string.Empty, "Font colour", string.Empty, "change_palette(this);", "Font colour: [color=red]text[/color]  Tip: you can also use color=#FF0000");

			sb.AppendLine(@"</div>");

			// Smilies
			sb.AppendLine(@"<div id=""smilies"">");
			sb.AppendLine(@"<h4 class=""redTitle"">Smilies</h4>");
			foreach (BBCodeHelper.Smiley smiley in BBCodeHelper.Smilies)
				AddSmiley(sb, smiley);
			sb.AppendLine(@"</div>");

			// Message textarea
			sb.AppendLine(@"<div>");
			sb.AppendLine(html.TextArea(name, new { onselect = "storeCaret(this);", onclick = "storeCaret(this);", onkeyup = "storeCaret(this);" }).ToString());
			sb.AppendLine(@"</div>");

			return sb.ToString();
		}

		private static void AddButton(StringBuilder sb, string accessKey, string value, string style, string onclick, string title)
		{
			sb.AppendLineFormat(@"<input type=""button"" class=""button2"" accesskey=""{0}"" value=""{1}"" style=""{2}"" onclick=""{3}; return false;"" title=""{4}"" />",
				accessKey, value, style, onclick, title);
		}

		private static void AddSmiley(StringBuilder sb, BBCodeHelper.Smiley smiley)
		{
			sb.AppendLineFormat(@"<a href=""#"" onclick=""{0}""><img src=""{1}"" width=""{2}"" height=""{3}"" alt=""{4}"" title=""{5}"" /></a>",
				"insert_text('" + smiley.Text + "', true); return false;", smiley.ImageUrl, smiley.Width, smiley.Height, smiley.Text, smiley.Title);
		}
	}
}