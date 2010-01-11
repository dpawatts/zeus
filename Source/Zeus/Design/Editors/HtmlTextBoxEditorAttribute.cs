using System;
using System.Web.UI.WebControls;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	[AttributeUsage(AttributeTargets.Property)]
	public class HtmlTextBoxEditorAttribute : TextBoxEditorAttribute
	{
		public HtmlTextBoxEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		public HtmlTextBoxEditorAttribute()
		{
		}

		public bool DomainAbsoluteUrls { get; set; }
		public string RootHtmlElementID { get; set; }

		protected override void ModifyEditor(TextBox tb)
		{
			tb.CssClass = string.Empty;
		}

		protected override TextBox CreateEditor()
		{
			return new HtmlTextBox { DomainAbsoluteUrls = DomainAbsoluteUrls, RootHtmlElementID = RootHtmlElementID };
		}
	}
}