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
			
		}

		protected override TextBox CreateEditor()
		{
			return new ColourPickerTextBox();
		}
	}
}
