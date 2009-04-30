using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Zeus.AddIns.Forums.Web.UI.WebControls
{
	[ValidationProperty("Text")]
	public class BBCodeEditor : WebControl, INamingContainer
	{
		private TextBox txtMessage;

		protected override HtmlTextWriterTag TagKey
		{
			get { return HtmlTextWriterTag.Div; }
		}

		public string Text
		{
			get { return txtMessage.Text; }
			set
			{
				EnsureChildControls();
				txtMessage.Text = value;
			}
		}

		protected override void CreateChildControls()
		{
			// Colour palette
			HtmlGenericControl colourPaletteDiv = new HtmlGenericControl("div") { ID = "colour_palette" };
			colourPaletteDiv.Style.Value = "display:none";
			Controls.Add(colourPaletteDiv);

			HtmlGenericControl dl = new HtmlGenericControl("dl");
			dl.Style.Value = "clear:left";

			HtmlGenericControl dt = new HtmlGenericControl("dt");
			dt.Controls.Add(new LiteralControl("<label>Font colour:</label>"));
			dl.Controls.Add(dt);

			HtmlGenericControl dd = new HtmlGenericControl("dd");
			string script = string.Format(@"<script type=""text/javascript"">
			function change_palette(link)
			{{
				$('#{0}').toggle();
				link.value = ($('#{0}').css('display') == 'block') ? 'Hide font colour' : 'Font colour';
			}}
			colorPalette('h', 15, 10);
		</script>", colourPaletteDiv.ClientID);
			dd.Controls.Add(new LiteralControl(script));
			dl.Controls.Add(dd);

			colourPaletteDiv.Controls.Add(dl);

			// Buttons
			HtmlGenericControl buttonsDiv = new HtmlGenericControl("div");
			AddButton(buttonsDiv, "b", " B ", "font-weight:bold; width: 30px", "bbstyle(0)", "Bold text: [b]text[/b]");
			AddButton(buttonsDiv, "i", " i ", "font-style:italic; width: 30px", "bbstyle(2)", "Italic text: [i]text[/i]");
			AddButton(buttonsDiv, "u", " u ", "text-decoration:underline; width: 30px", "bbstyle(4)", "Underline text: [u]text[/u]");
			AddButton(buttonsDiv, "q", "Quote", "width: 50px", "bbstyle(6)", "Quote text: [quote]text[/quote]");
			AddButton(buttonsDiv, "c", "Code", "width: 40px", "bbstyle(8)", "Code display: [code]text[/code]");
			AddButton(buttonsDiv, "l", "List", "width: 40px", "bbstyle(10)", "List: [list]text[/list]");
			AddButton(buttonsDiv, "o", "List=", "width: 40px", "bbstyle(12)", "Ordered list: [list=]text[/list]");
			AddButton(buttonsDiv, "t", "[*]", "width: 40px", "bbstyle(-1)", "List item: [*]text[/*]");
			AddButton(buttonsDiv, "p", "Img", "width: 40px", "bbstyle(14)", "Insert image: [img]http://image_url[/img]");
			AddButton(buttonsDiv, "w", "URL", "text-decoration:underline; width: 40px", "bbstyle(16)", "Insert URL: [url]http://url[/url] or [url=http://url]URL text[/url]");
			AddButton(buttonsDiv, "d", "Flash", string.Empty, "bbstyle(18)", "Flash: [flash=width,height]http://url[/flash]");

			HtmlSelect fontSelect = new HtmlSelect();
			fontSelect.Attributes["onchange"] = "bbfontstyle('[size=' + $(this).val() + ']', '[/size]'); $(this).val('100');";
			fontSelect.Attributes["title"] = "Font size: [size=85]small text[/size]";
			fontSelect.Items.Add(new ListItem("Tiny", "50"));
			fontSelect.Items.Add(new ListItem("Small", "85"));
			fontSelect.Items.Add(new ListItem("Normal", "100") { Selected = true });
			fontSelect.Items.Add(new ListItem("Large", "150"));
			fontSelect.Items.Add(new ListItem("Huge", "200"));
			buttonsDiv.Controls.Add(fontSelect);

			AddButton(buttonsDiv, string.Empty, "Font colour", string.Empty, "change_palette(this);", "Font colour: [color=red]text[/color]  Tip: you can also use color=#FF0000");

			Controls.Add(buttonsDiv);

			// Smiley box
			HtmlGenericControl smiliesDiv = new HtmlGenericControl("div");
			smiliesDiv.Controls.Add(new LiteralControl("<strong>Smilies</strong><br />"));
			foreach (BBCodeHelper.Smiley smiley in BBCodeHelper.Smilies)
				AddSmiley(smiliesDiv, smiley);
			Controls.Add(smiliesDiv);

			// Message textbox
			HtmlGenericControl messageDiv = new HtmlGenericControl("div");
			txtMessage = new TextBox
			             	{
			             		ID = "txtMessage",
			             		Rows = 15,
			             		Columns = 76,
			             		CssClass = "inputbox",
			             		TextMode = TextBoxMode.MultiLine
			             	};
			txtMessage.Attributes["onselect"] = "storeCaret(this);";
			txtMessage.Attributes["onclick"] = "storeCaret(this);";
			txtMessage.Attributes["onkeyup"] = "storeCaret(this);";
			messageDiv.Controls.Add(txtMessage);
			Controls.Add(messageDiv);
		}

		private void AddSmiley(Control container, BBCodeHelper.Smiley smiley)
		{
			HtmlAnchor a = new HtmlAnchor { HRef = "#" };
			a.Attributes["onclick"] = "insert_text('" + smiley.Text + "', true); return false;";

			HtmlImage img = new HtmlImage
			                	{
													Src = smiley.ImageUrl,
													Width = smiley.Width,
													Height = smiley.Height,
													Alt = smiley.Text
			                	};
			img.Attributes["title"] = smiley.Title;
			a.Controls.Add(img);

			container.Controls.Add(a);
		}

		private static void AddButton(Control container, string accessKey, string value, string style, string onclick, string title)
		{
			HtmlInputButton button = new HtmlInputButton();
			button.Attributes["class"] = "button2";
			button.Attributes["accesskey"] = accessKey;
			button.Value = value;
			button.Style.Value = style;
			button.Attributes["onclick"] = onclick;
			button.Attributes["title"] = title;
			container.Controls.Add(button);
		}

		protected override void OnPreRender(System.EventArgs e)
		{
			base.OnPreRender(e);

			Page.ClientScript.RegisterClientScriptResource(typeof(BBCodeEditor), "Zeus.AddIns.Forums.Web.Resources.BBCodeEditor.js");

			string script = string.Format(@"
	var onload_functions = new Array();

	var form_name = '{0}';
	var text_name = '{1}';
	//var load_draft = false;
	//var upload = false;

	// Define the bbCode tags
	//var bbcode = new Array();
	var bbtags = new Array('[b]','[/b]','[i]','[/i]','[u]','[/u]','[quote]','[/quote]','[code]','[/code]','[list]','[/list]','[list=]','[/list]','[img]','[/img]','[url]','[/url]','[flash=]', '[/flash]','[size=]','[/size]');
	//var imageTag = false;

	// Helpline messages
	var help_line = {{
		b: 'Bold text: [b]text[/b]',
		i: 'Italic text: [i]text[/i]',
		u: 'Underline text: [u]text[/u]',
		q: 'Quote text: [quote]text[/quote]',
		c: 'Code display: [code]code[/code]',
		l: 'List: [list]text[/list]',
		o: 'Ordered list: [list=]text[/list]',
		p: 'Insert image: [img]http://image_url[/img]',
		w: 'Insert URL: [url]http://url[/url] or [url=http://url]URL text[/url]',
		a: 'Inline uploaded attachment: [attachment=]filename.ext[/attachment]',
		s: 'Font colour: [color=red]text[/color]  Tip: you can also use color=#FF0000',
		f: 'Font size: [size=85]small text[/size]',
		e: 'List: Add list element',
		d: 'Flash: [flash=width,height]http://url[/flash]'
			}}
", Page.Form.ClientID, txtMessage.ClientID);
			Page.ClientScript.RegisterStartupScript(typeof(BBCodeEditor), ClientID, script, true);
		}
	}
}