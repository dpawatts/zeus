using System;
using Zeus.Design.Editors;

namespace Zeus.ContentProperties
{
	[DoublePropertyDataType]
	public class DoubleProperty : PropertyData
	{
		#region Constuctors

		public DoubleProperty()
		{
		}

		public DoubleProperty(ContentItem containerItem, string name, double value)
			: base(containerItem, name)
		{
			DoubleValue = value;
		}

		#endregion

		public virtual double DoubleValue { get; set; }

		public override PropertyDataType Type
		{
			get { return PropertyDataType.Double; }
		}

		public override object Value
		{
			get { return DoubleValue; }
			set { DoubleValue = (double) value; }
		}

		public override Type ValueType
		{
			get { return typeof(double); }
		}

		public override IEditor GetDefaultEditor(string title, int sortOrder, Type propertyType)
		{
			return new TextBoxEditorAttribute(title, sortOrder, 10);
		}
	}
}