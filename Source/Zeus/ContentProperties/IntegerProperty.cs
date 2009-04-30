using System;
using Zeus.Design.Editors;

namespace Zeus.ContentProperties
{
	public class IntegerProperty : PropertyData
	{
		#region Constuctors

		public IntegerProperty()
		{
		}

		public IntegerProperty(ContentItem containerItem, string name, int value)
			: base(containerItem, name)
		{
			IntegerValue = value;
		}

		#endregion

		public override string ColumnName
		{
			get { return "IntValue"; }
		}

		public virtual int IntegerValue { get; set; }

		public override PropertyDataType Type
		{
			get { return PropertyDataType.Integer; }
		}

		public override object Value
		{
			get { return IntegerValue; }
			set { IntegerValue = (int) value; }
		}

		public override Type ValueType
		{
			get { return typeof(int); }
		}

		public override IEditor GetDefaultEditor(string title, int sortOrder, Type propertyType)
		{
			return new TextBoxEditorAttribute(title, sortOrder, 10);
		}
	}
}