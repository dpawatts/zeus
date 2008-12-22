using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using Zeus.Integrity;

[assembly: WebResource("Zeus.Web.UI.WebControls.NameEditor.js", "text/javascript")]
namespace Zeus.Web.UI.WebControls
{
	public class NameEditor : TextBox, IValidator
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

		#region IValidator Members

		/// <summary>Gets or sets the error message generated when the name editor contains invalid values.</summary>
		public string ErrorMessage
		{
			get { return (string) ViewState["ErrorMessage"] ?? ""; }
			set { ViewState["ErrorMessage"] = value; }
		}

		/// <summary>Gets or sets wether the name editor's value passes validaton.</summary>
		public bool IsValid
		{
			get { return ViewState["IsValid"] != null ? (bool) ViewState["IsValid"] : true; }
			set { ViewState["IsValid"] = value; }
		}

		/// <summary>Validates the name editor's value checking uniqueness and lenght.</summary>
		public void Validate()
		{
			ContentItem currentItem = this.FindCurrentItem();

			if (currentItem != null)
			{
				if (!string.IsNullOrEmpty(Text))
				{
					// Ensure that the chosen name is locally unique
					if (!Zeus.Context.Current.Resolve<IIntegrityManager>().IsLocallyUnique(Text, currentItem))
					{
						//Another item with the same parent and the same name was found 
						ErrorMessage = string.Format("Another item already has the name '{0}'.", Text);
						IsValid = false;
						return;
					}

					// Ensure that the path isn't longer than 260 characters
					if (currentItem.Parent != null && (currentItem.Parent.Url.Length > 248 || currentItem.Parent.Url.Length + Text.Length > 260))
					{
						ErrorMessage = string.Format("Name too long, the full url cannot exceed 260 characters: {0}", Text);
						IsValid = false;
						return;
					}
				}
			}

			IsValid = true;
		}

		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			base.RenderBeginTag(writer);
		}

		/// <summary>Renders an additional asterix when validation didn't pass.</summary>
		/// <param name="writer">The writer to render the asterix to.</param>
		public override void RenderEndTag(HtmlTextWriter writer)
		{
			base.RenderEndTag(writer);
			if (!IsValid)
				writer.Write("<span style='color:red'>*</span>");
		}

		#endregion
	}
}
