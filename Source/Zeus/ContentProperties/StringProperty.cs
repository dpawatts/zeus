using System;
using Zeus.Design.Editors;

namespace Zeus.ContentProperties
{
	public class StringProperty : PropertyData
	{
		#region Constuctors

		public StringProperty()
		{
		}

		public StringProperty(ContentItem containerItem, string name, string value)
			: base(containerItem, name)
		{
			StringValue = value;
		}

		#endregion

		public virtual string StringValue { get; set; }

		public override PropertyDataType Type
		{
			get { return PropertyDataType.String; }
		}

		public override object Value
		{
			get { return StringValue; }
			set { StringValue = (string) value; }
		}

		public override Type ValueType
		{
			get { return typeof(string); }
		}

		public override IEditor GetDefaultEditor(string title, int sortOrder, Type propertyType)
		{
			return new TextBoxEditorAttribute(title, sortOrder);
		}
	}
}