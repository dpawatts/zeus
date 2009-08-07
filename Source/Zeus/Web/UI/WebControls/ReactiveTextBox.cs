using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using Zeus.Integrity;

[assembly: WebResource("Zeus.Web.UI.WebControls.ReactiveTextBox.js", "text/javascript")]
namespace Zeus.Web.UI.WebControls
{
	public class ReactiveTextBox : TextBox
	{
		private CheckBox chkKeepUpdated;

		public string FormatString
		{
			get { return (string) ViewState["FormatString"]; }
			set { ViewState["FormatString"] = value; }
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			chkKeepUpdated = new CheckBox { ID = ID + "_chkKeepUpdated", Text = "Keep updated" };
			Controls.Add(new LiteralControl("&nbsp;"));
			Controls.Add(chkKeepUpdated);
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			// Render javascript for updating name textbox based on title textbox.
			Page.ClientScript.RegisterClientScriptResource(typeof(ReactiveTextBox), "Zeus.Web.UI.WebControls.ReactiveTextBox.js");

			List<string> otherEditors = new List<string>();
			foreach (KeyValuePair<string, Control> propertyControl in Parent.FindParent<ItemView>().PropertyControls)
				otherEditors.Add("'" + propertyControl.Key + "' : '" + propertyControl.Value.ClientID + "'");
			string reactiveOptions = string.Format(@"{{formatString: '{0}', keepUpdatedClientID: '{1}', otherEditors: {{{2}}} }}",
				FormatString, chkKeepUpdated.ClientID, string.Join(", ", otherEditors.ToArray()));
			string script = string.Format(@"jQuery(document).ready(function() {{
					jQuery('#{0}').reactiveTextBox({1});
					var chkKeepUpdated = document.getElementById('{2}');
					var value1 = jQuery.fn.reactiveTextBox.formattedValue({1});
					var value2 = jQuery('#{0}').val();
					chkKeepUpdated.checked = (value1 == value2 || value2 == '');
				}});", ClientID, reactiveOptions, chkKeepUpdated.ClientID);
			Page.ClientScript.RegisterStartupScript(typeof(ReactiveTextBox), ClientID, script, true);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			// Copied from TextBox.Render() using Reflector
			RenderBeginTag(writer);
			if (TextMode == TextBoxMode.MultiLine)
				HttpUtility.HtmlEncode(Text, writer);
			RenderEndTag(writer);

			// Needed to write out chkKeepUpdated
			RenderContents(writer);
		}
	}
}
