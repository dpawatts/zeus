using System.Web.UI.WebControls;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	public class ReactiveTextBoxEditorAttribute : TextBoxEditorAttribute
	{
		public ReactiveTextBoxEditorAttribute(string title, int sortOrder, string formatString)
			: base(title, sortOrder)
		{
			FormatString = formatString;
		}

		public ReactiveTextBoxEditorAttribute(string formatString)
		{
			FormatString = formatString;
		}

		public string FormatString { get; set; }

		protected override TextBox CreateEditor()
		{
			return new ReactiveTextBox { FormatString = FormatString };
		}
	}
}