using System;
using System.Web.UI.WebControls;
using System.Web.UI;

[assembly: WebResource("Zeus.Web.UI.WebControls.NameEditor.js", "text/javascript")]
namespace Zeus.Web.UI.WebControls
{
	public class NameEditor : TextBox
	{
		public NameEditor()
		{
			this.MaxLength = 250;
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			// Render javascript for updating name textbox based on title textbox.
			this.Page.ClientScript.RegisterClientScriptResource(typeof(NameEditor), "Zeus.Web.UI.WebControls.NameEditor.js");

			ItemView itemView = this.Parent.FindParent<ItemView>();
			Control titleEditor = itemView.PropertyControls["Title"];
			string script = string.Format(@"$(document).ready(function() {{
					$('#{0}').nameEditor({{titleEditorID: '{1}'}});
				}});", this.ClientID, titleEditor.ClientID);
			this.Page.ClientScript.RegisterStartupScript(typeof(NameEditor), "InitNameEditor", script, true);
		}
	}
}
