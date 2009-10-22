using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class ComboBox : CompositeControl
	{
		private HtmlGenericControl _ul;
		private TextBox _textBox;

		protected override void CreateChildControls()
		{
			_ul = new HtmlGenericControl("ul");
			_ul.ID = "categoryMenu";
			_ul.Attributes["class"] = "mcdropdown_menu";
			_ul.InnerHtml = @"<li rel=""1"">
		Arts &amp; Humanities
		<ul>
			<li rel=""2"">
				Photography
				<ul>
					<li rel=""3"">
						3D
					</li>
					<li rel=""4"">
						Digital
					</li>
				</ul>
			</li>
			<li rel=""5"">
				History
			</li>
			<li rel=""6"">Literature</li>
		</ul>
	</li>
	<li rel=""7"">
		Business &amp; Economy
	</li>
	<li rel=""8"">
		Computers &amp; Internet
	</li>
	<li rel=""9"">
		Education
	</li>
	<li rel=""11"">
		Entertainment
		<ul>
			<li rel=""12"">
				Movies
			</li>
			<li rel=""13"">
				TV Shows
			</li>
			<li rel=""14"">
				Music
			</li>
			<li rel=""15"">
				Humor
			</li>
		</ul>
	</li>
	<li rel=""10"">
		Health
	</li>";
			Controls.Add(_ul);

			_textBox = new TextBox();
			_textBox.ID = "txtTextBox";
			Controls.Add(_textBox);

			base.CreateChildControls();
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterJavascriptResource(typeof(ComboBox), "Zeus.Web.Resources.jQuery.Plugins.jquery.mcdropdown.js");
			Page.ClientScript.RegisterCssResource(GetType(), "Zeus.Web.Resources.jQuery.Plugins.jquery.mcdropdown.css");

			string script = @"$(document).ready(function (){
				$('#" + _textBox.ClientID + @"').mcDropdown('#" + _ul.ClientID + @"');
			});";
			Page.ClientScript.RegisterStartupScript(typeof(ComboBox), ClientID, script, true);

			base.OnPreRender(e);
		}
	}
}
