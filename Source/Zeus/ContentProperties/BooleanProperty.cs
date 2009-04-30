using System;
using Zeus.Design.Editors;

namespace Zeus.ContentProperties
{
	public class BooleanProperty : PropertyData
	{
		#region Constuctors

		public BooleanProperty()
		{
		}

		public BooleanProperty(ContentItem containerItem, string name, bool value)
			: base(containerItem, name)
		{
			BooleanValue = value;
		}

		#endregion

		public virtual bool BooleanValue { get; set; }

		public override string ColumnName
		{
			get { return "BoolValue"; }
		}

		public override PropertyDataType Type
		{
			get { return PropertyDataType.Boolean; }
		}

		public override object Value
		{
			get { return BooleanValue; }
			set { BooleanValue = (bool) value; }
		}

		public override Type ValueType
		{
			get { return typeof(bool); }
		}

		public override IEditor GetDefaultEditor(string title, int sortOrder, Type propertyType)
		{
			return new CheckBoxEditorAttribute(title, string.Empty, sortOrder);
		}
	}
}