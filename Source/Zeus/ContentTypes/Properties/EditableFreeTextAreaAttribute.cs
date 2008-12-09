using System;
using System.Web.UI.WebControls;

namespace Zeus.ContentTypes.Properties
{
	[AttributeUsage(AttributeTargets.Property)]
	public class HtmlTextBoxEditorAttribute : EditableTextBoxAttribute
	{
		public HtmlTextBoxEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override void ModifyEditor(TextBox tb)
		{
			// do nothing
		}

		protected override TextBox CreateEditor()
		{
			return new HtmlTextBox();
		}
	}
}
