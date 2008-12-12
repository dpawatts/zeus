using System;
using System.Web.UI.WebControls;
using Isis.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class ColourPickerTextBox : TextBox
	{
		public ColourPickerTextBox()
		{
			this.CssClass = "colourPicker";
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			this.Page.ClientScript.RegisterClientScriptInclude("ColourPicker", "/admin/assets/js/plugins/jquery.colorpicker.js");
			this.Page.RegisterCssInclude("~/admin/assets/css/colorpicker.css");

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
			this.Page.ClientScript.RegisterStartupScript(typeof(ColourPickerTextBox), "ColourPicker", script, true);
		}
	}
}
