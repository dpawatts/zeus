using System;
using Zeus.Design.Editors;

namespace Zeus.ContentProperties
{
	[PropertyDataType(typeof(bool))]
	public class BooleanProperty : PropertyData
	{
		#region Constuctors

		public BooleanProperty()
		{
		}

		public BooleanProperty(ContentItem containerItem, string name, bool value)
			: base(containerItem, name)
		{
			BoolValue = value;
		}

		#endregion

		public virtual bool BoolValue { get; set; }

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
			get { return BoolValue; }
			set { BoolValue = (bool) value; }
		}

		public override Type ValueType
		{
			get { return typeof(bool); }
		}

		public override IEditor GetDefaultEditor(string title, int sortOrder, Type propertyType, string containerName)
		{
			return new CheckBoxEditorAttribute(title, string.Empty, sortOrder) { ContainerName = containerName };
		}
	}
}