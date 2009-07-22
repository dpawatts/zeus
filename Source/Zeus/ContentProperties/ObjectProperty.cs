using System;
using Zeus.Design.Editors;

namespace Zeus.ContentProperties
{
	[ObjectPropertyDataType]
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

		public virtual ObjectPropertyDataBlob Blob { get; set; }

		public override object Value
		{
			get { return (Blob != null) ? Blob.Blob : null; }
			set
			{
				if (Blob == null)
					Blob = new ObjectPropertyDataBlob();
				Blob.Blob = value;
			}
		}

		public override Type ValueType
		{
			get { return typeof(object); }
		}

		#endregion

		#region Methods

		public override IEditor GetDefaultEditor(string title, int sortOrder, Type propertyType, string containerName)
		{
			return null;
		}

		#endregion
	}
}