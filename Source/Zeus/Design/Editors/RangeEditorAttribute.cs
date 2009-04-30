using System;
using System.Linq;
using System.Web.UI.WebControls;
using Zeus.ContentTypes;

namespace Zeus.Design.Editors
{
	public class RangeEditorAttribute : DropDownListEditorAttribute
	{
		private readonly int _min, _max;

		public RangeEditorAttribute(string title, int sortOrder, int min, int max)
			: base(title, sortOrder)
		{
			Required = true;
			_min = min;
			_max = max;
		}

		protected override ListItem[] GetListItems(IEditableObject contentItem)
		{
			return Enumerable.Range(_min, _max - _min).Select(i => new ListItem(i.ToString())).ToArray();
		}

		protected override object GetValue(IEditableObject item)
		{
			return item[Name];
		}

		protected override object GetValue(ListControl ddl)
		{
			if (!string.IsNullOrEmpty(ddl.SelectedValue))
				return int.Parse(ddl.SelectedValue);
			return null;
		}
	}
}
