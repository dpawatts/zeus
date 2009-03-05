using System;
using System.Web.UI.WebControls;
using Zeus.Web.UI.WebControls;

namespace Zeus.ContentTypes.Properties
{
	public class ColourEditorAttribute : TextBoxEditorAttribute
	{
		public ColourEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder, 6)
		{
			ValidateRegularExpression = true;
			ValidationExpression = "[a-zA-Z0-9]+";
			ValidationMessage = Title + " must contain only letters and numbers and no other characters (such as '#')";
			EditorPrefixText = "#&nbsp;";
			TextBoxCssClass = "colour";
		}

		protected override TextBox CreateEditor()
		{
			return new ColourPickerTextBox();
		}
	}
}
