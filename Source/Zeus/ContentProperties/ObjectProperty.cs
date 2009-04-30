using System;
using Zeus.Design.Editors;

namespace Zeus.ContentProperties
{
	public class ObjectProperty : PropertyData
	{
		#region Constuctors

		public ObjectProperty()
		{
		}

		public ObjectProperty(ContentItem containerItem, string name, object value)
			: base(containerItem, name)
		{
			Value = value;
		}

		#endregion

		#region Properties

		public override string ColumnName
		{
			get { return "Value"; }
		}

		public override PropertyDataType Type
		{
			get { return PropertyDataType.Object; }
		}

		public override object Value { get; set; }

		public override Type ValueType
		{
			get { return typeof(object); }
		}

		#endregion

		#region Methods

		public override IEditor GetDefaultEditor(string title, int sortOrder, Type propertyType)
		{
			throw new NotSupportedException();
		}

		#endregion
	}
}