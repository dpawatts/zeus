using System;
using System.Web.UI.WebControls;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	public class SortableLinkedItemsCheckBoxListEditorAttribute : LinkedItemsCheckBoxListEditorAttribute
	{
		public SortableLinkedItemsCheckBoxListEditorAttribute()
		{
		}

		public SortableLinkedItemsCheckBoxListEditorAttribute(string title, int sortOrder, Type typeFilter)
			: base(title, sortOrder, typeFilter)
		{
		}

		protected override CheckBoxList CreateEditor()
		{
			return new SortableCheckBoxList();
		}
	}
}