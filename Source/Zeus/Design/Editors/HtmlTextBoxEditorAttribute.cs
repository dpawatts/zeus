using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	[AttributeUsage(AttributeTargets.Property)]
	public class HtmlTextBoxEditorAttribute : TextEditorAttributeBase
	{
		public HtmlTextBoxEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		public HtmlTextBoxEditorAttribute(string title, int sortOrder, int maxLength)
			: base(title, sortOrder, maxLength)
		{
		}

		public HtmlTextBoxEditorAttribute()
		{
		}

		public bool DomainAbsoluteUrls { get; set; }
		public string RootHtmlElementID { get; set; }
		public string CustomCssUrl { get; set; }
		public string CustomStyleList { get; set; }

		protected override void DisableEditor(Control editor)
		{
			((HtmlTextBox) editor).Enabled = false;
			((HtmlTextBox) editor).ReadOnly = true;
		}

		/// <summary>Creates a text box editor.</summary>
		/// <param name="container">The container control the tetx box will be placed in.</param>
		/// <returns>A text box control.</returns>
		protected override Control AddEditor(Control container)
		{
			HtmlTextBox tb = new HtmlTextBox
			{
				DomainAbsoluteUrls = DomainAbsoluteUrls,
				RootHtmlElementID = RootHtmlElementID,
				CustomCssUrl = CustomCssUrl,
				CustomStyleList = CustomStyleList
			};
			tb.ID = Name;
			if (Required)
				tb.CssClass += " required";
			if (ReadOnly)
				tb.ReadOnly = true;
			container.Controls.Add(tb);

			return tb;
		}
	}
}