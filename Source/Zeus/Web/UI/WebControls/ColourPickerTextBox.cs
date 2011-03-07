using System;
using System.Web.UI.WebControls;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class ColourPickerTextBox : TextBox
	{
		public ColourPickerTextBox()
		{
			CssClass = "colourPicker";
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			Page.ClientScript.RegisterJavascriptResource(typeof(ColourPickerTextBox), "Zeus.Web.Resources.jQuery.Plugins.jquery.colorpicker.js");
			Page.ClientScript.RegisterCssResource(GetType(), "Zeus.Web.Resources.jQuery.Plugins.jquery.colorpicker.css");

			string script = @"$('#" + this.ClientID + @"').ColorPicker({
	onSubmit: function(hsb, hex, rgb) {
		$('#" + this.ClientID + @"').val(hex);
	},
	onBeforeShow: function() {
		$(this).ColorPickerSetColor(this.value);
	}
})
.bind('keyup', function() {
	$(this).ColorPickerSetColor(this.value);
});";
			Page.ClientScript.RegisterStartupScript(typeof(ColourPickerTextBox), ClientID, script, true);
		}
	}
}
