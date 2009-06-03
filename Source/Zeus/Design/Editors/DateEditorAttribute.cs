using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.ContentTypes;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	public class DateEditorAttribute : TextBoxEditorAttribute
	{
		public DateEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder, 10)
		{
		}

		public DateEditorAttribute()
		{
			
		}

		protected override TextBox CreateEditor()
		{
			return new DatePicker();
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			DatePicker tb = editor as DatePicker;
			if (item[Name] != null)
				tb.Text = ((DateTime) item[Name]).ToShortDateString();
		}
	}
}