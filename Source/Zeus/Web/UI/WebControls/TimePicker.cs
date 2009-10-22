using System;
using System.Web.UI.WebControls;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public sealed class TimePicker : TextBox
	{
		public TimePicker()
		{
			CssClass = "timePicker";
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			Page.ClientScript.RegisterClientScriptResource(typeof(TimePicker),
				"Zeus.Web.Resources.jQuery.Plugins.jquery.timePicker.js");
			Page.ClientScript.RegisterStartupScript(typeof(TimePicker), ClientID,
				@"if (!$.browser.msie) $('#" + ClientID + @"').timePicker({step:30});", true);

			Page.ClientScript.RegisterCssResource(GetType(), "Zeus.Web.Resources.jQuery.Plugins.jquery.timePicker.css");
			Page.ClientScript.RegisterCssResource(GetType(), "Zeus.Web.Resources.jQuery.TimePicker.css");
		}
	}
}
