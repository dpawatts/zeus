using System;
using System.Web.UI.WebControls;
using Zeus.Web.UI.WebControls;

namespace Zeus.ContentTypes.Properties
{
	public class DateEditorAttribute : TextBoxEditorAttribute
	{
		public DateEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder, 10)
		{
			
		}

		protected override TextBox CreateEditor()
		{
			return new DateEditorTextBox();
		}

		public override void UpdateEditor(ContentItem item, System.Web.UI.Control editor)
		{
			TextBox tb = editor as TextBox;
			tb.Text = ((DateTime) item[Name]).ToShortDateString();
		}
	}
}
