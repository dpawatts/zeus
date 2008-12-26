using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using Zeus.Integrity;

[assembly: WebResource("Zeus.Web.UI.WebControls.NameEditor.js", "text/javascript")]
namespace Zeus.Web.UI.WebControls
{
	[ValidationProperty("Text")]
	public class NameEditor : CompositeControl, IValidator
	{
		private Label _label;
		private TextBox _textBox;
		private Panel _labelPanel, _editPanel;

		public string Text
		{
			get { return _textBox.Text; }
			set { EnsureChildControls(); _textBox.Text = value; }
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			_labelPanel = new Panel();
			_labelPanel.ID = this.ID + "labelPanel";
			this.Controls.Add(_labelPanel);

			_label = new Label();
			_label.Text = this.FindCurrentItem().Name;
			_label.CssClass = "nameLabel";
			_label.ID = _labelPanel.ID + "label";
			_labelPanel.Controls.Add(_label);

			_editPanel = new Panel();
			_editPanel.ID = this.ID + "editPanel";
			_editPanel.Style[HtmlTextWriterStyle.Display] = "none";
			this.Controls.Add(_editPanel);

			_textBox = new TextBox();
			_textBox.MaxLength = 250;
			_textBox.ID = _editPanel.ID + "textBox";
			_editPanel.Controls.Add(_textBox);
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			// Render javascript for updating name textbox based on title textbox.
			this.Page.ClientScript.RegisterClientScriptResource(typeof(NameEditor), "Zeus.Web.UI.WebControls.NameEditor.js");

			_labelPanel.Controls.Add(new LiteralControl("<br /><span class=\"edit\"><a href=\"#\" onclick=\"$('#" + _labelPanel.ClientID + "').hide();$('#" + _editPanel.ClientID + "').show();return false;\">Edit</a></span>"));

			ItemView itemView = this.Parent.FindParent<ItemView>();
			Control titleEditor = itemView.PropertyControls["Title"];
			string script = string.Format(@"$(document).ready(function() {{
					$('#{0}, #{1}').nameEditor({{titleEditorID: '{2}'}});
				}});", _label.ClientID, _textBox.ClientID, titleEditor.ClientID);
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
				if (!string.IsNullOrEmpty(_textBox.Text))
				{
					// Ensure that the chosen name is locally unique
					if (!Zeus.Context.Current.Resolve<IIntegrityManager>().IsLocallyUnique(_textBox.Text, currentItem))
					{
						//Another item with the same parent and the same name was found 
						ErrorMessage = string.Format("Another item already has the name '{0}'.", _textBox.Text);
						IsValid = false;
						return;
					}

					// Ensure that the path isn't longer than 260 characters
					if (currentItem.Parent != null && (currentItem.Parent.Url.Length > 248 || currentItem.Parent.Url.Length + _textBox.Text.Length > 260))
					{
						ErrorMessage = string.Format("Name too long, the full url cannot exceed 260 characters: {0}", _textBox.Text);
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
