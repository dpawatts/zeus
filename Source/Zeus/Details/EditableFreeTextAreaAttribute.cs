using System;
using System.Web.UI.WebControls;

namespace Zeus.Details
{
	[AttributeUsage(AttributeTargets.Property)]
	public class EditableFreeTextAreaAttribute : EditableTextBoxAttribute
	{
		public EditableFreeTextAreaAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		protected override void ModifyEditor(TextBox tb)
		{
			// do nothing
		}

		protected override TextBox CreateEditor()
		{
			//return new FreeTextArea();
			return new TextBox();
		}
	}
}
