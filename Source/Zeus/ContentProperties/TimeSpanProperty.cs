using System;
using Zeus.Design.Editors;

namespace Zeus.ContentProperties
{
	[PropertyDataType(typeof(TimeSpan))]
	public class TimeSpanProperty : PropertyData
	{
		#region Constuctors

		public TimeSpanProperty()
		{
		}

		public TimeSpanProperty(ContentItem containerItem, string name, TimeSpan value)
			: base(containerItem, name)
		{
			TimeSpanValue = value;
		}

		#endregion

		public virtual TimeSpan TimeSpanValue { get; set; }

		public override PropertyDataType Type
		{
			get { return PropertyDataType.TimeSpan; }
		}

		public override object Value
		{
			get { return TimeSpanValue; }
			set { TimeSpanValue = (TimeSpan) value; }
		}

		public override Type ValueType
		{
			get { return typeof(TimeSpan); }
		}

		public override IEditor GetDefaultEditor(string title, int sortOrder, Type propertyType, string containerName)
		{
			return new TextBoxEditorAttribute(title, sortOrder) { ContainerName = containerName };
		}
	}
}