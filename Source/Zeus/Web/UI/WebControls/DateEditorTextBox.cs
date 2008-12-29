using System;
using System.Web.UI.WebControls;
using Isis.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class DateEditorTextBox : TextBox
	{
		public DateEditorTextBox()
		{
			this.CssClass = "dateEditor";
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			this.Page.ClientScript.RegisterClientScriptInclude("DatePicker", "/admin/assets/js/plugins/ui.datepicker.js");
			this.Page.RegisterCssInclude("~/admin/assets/css/ui.datepicker.css");

			string script = @"$('#" + this.ClientID + @"').datepicker({ 
    duration: '', 
    showOn: 'both', 
    buttonImage: '/admin/assets/images/datepicker/calendar.gif',
    buttonImageOnly: true });";
			this.Page.ClientScript.RegisterStartupScript(typeof(DateEditorTextBox), this.ClientID, script, true);
		}
	}
}
