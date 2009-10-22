using System;
using System.Web.UI.WebControls;
using Zeus.BaseLibrary;
using Zeus.ContentTypes;

namespace Zeus.Design.Editors
{
	public class EnumEditorAttribute : DropDownListEditorAttribute
	{
		private readonly Type _enumType;

		public EnumEditorAttribute(string title, int sortOrder, Type enumType)
			: base(title, sortOrder)
		{
			if (enumType == null) throw new ArgumentNullException("enumType");
			if (!enumType.IsEnum) throw new ArgumentException("The parameter 'enumType' is not a type of enum.", "enumType");

			Required = true;
			_enumType = enumType;
		}

		public EnumEditorAttribute(Type enumType)
		{
			if (enumType == null) throw new ArgumentNullException("enumType");
			if (!enumType.IsEnum) throw new ArgumentException("The parameter 'enumType' is not a type of enum.", "enumType");

			Required = true;
			_enumType = enumType;
		}

		protected override ListItem[] GetListItems(IEditableObject contentItem)
		{
			Array values = Enum.GetValues(_enumType);
			ListItem[] items = new ListItem[values.Length];
			for (int i = 0; i < values.Length; i++)
			{
				int value = (int) values.GetValue(i);
				string name = EnumHelper.GetEnumValueDescription(_enumType, Enum.GetName(_enumType, value));
				items[i] = new ListItem(name, value.ToString());
			}
			return items;
		}

		protected override object GetValue(IEditableObject item)
		{
			object value = item[Name];

			if (value == null)
				return null;

			if (value is string)
				// an enum as string we assume
				return ((int) Enum.Parse(_enumType, (string) value)).ToString();

			if (value is int)
				// an enum as int we hope
				return value.ToString();

			// hopefully an enum type;
			return ((int) value).ToString();
		}

		protected override object GetValue(ListControl ddl)
		{
			if (!string.IsNullOrEmpty(ddl.SelectedValue))
				return GetEnumValue(int.Parse(ddl.SelectedValue));
			return null;
		}

		private object GetEnumValue(int value)
		{
			foreach (object e in Enum.GetValues(_enumType))
				if ((int) e == value)
					return e;
			return null;
		}
	}
}
