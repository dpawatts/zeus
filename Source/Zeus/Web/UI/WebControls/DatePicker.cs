using System;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public sealed class DatePicker : TextBox
	{
		public DatePicker()
		{
			CssClass = "datePicker";
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			Page.ClientScript.RegisterJavascriptResource(typeof(DatePicker), "Zeus.Web.Resources.jQuery.ui.core.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(DatePicker), "Zeus.Web.Resources.jQuery.ui.datepicker.js");

			Page.ClientScript.RegisterEmbeddedCssResource(typeof(DatePicker), "Zeus.Web.Resources.jQuery.ui.css");
			Page.ClientScript.RegisterCssResource(typeof(DatePicker), "Zeus.Web.Resources.jQuery.DatePicker.css");

			string script = @"if (!jQuery.browser.msie) jQuery('#" + ClientID + @"').datepicker({ 
    duration: '', 
    showOn: 'both', 
    buttonImage: '" + Page.ClientScript.GetWebResourceUrl(GetType(), "Zeus.Web.Resources.jQuery.calendar.gif") + @"',
    buttonImageOnly: true,
		dateFormat: 'dd/mm/yy' });";
			Page.ClientScript.RegisterStartupScript(typeof(DatePicker), ClientID, script, true);
		}
	}
}
