using System;

namespace Zeus.ContentTypes.Properties
{
	public class NameEditorAttribute : EditableTextBoxAttribute
	{
		public NameEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder, 250)
		{

		}
	}
}
