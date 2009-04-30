using System;
using Zeus.Design.Editors;

namespace Zeus.ContentProperties
{
	public class DateTimeProperty : PropertyData
	{
		#region Constuctors

		public DateTimeProperty()
		{
		}

		public DateTimeProperty(ContentItem containerItem, string name, DateTime value)
			: base(containerItem, name)
		{
			DateTimeValue = value;
		}

		#endregion

		public virtual DateTime DateTimeValue { get; set; }

		public override PropertyDataType Type
		{
			get { return PropertyDataType.DateTime; }
		}

		public override object Value
		{
			get { return DateTimeValue; }
			set { DateTimeValue = (DateTime) value; }
		}

		public override Type ValueType
		{
			get { return typeof(DateTime); }
		}

		public override IEditor GetDefaultEditor(string title, int sortOrder, Type propertyType)
		{
			return new DateEditorAttribute(title, sortOrder);
		}
	}
}