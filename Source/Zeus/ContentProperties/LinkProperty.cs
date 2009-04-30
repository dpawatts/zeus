using System;
using Zeus.Design.Editors;

namespace Zeus.ContentProperties
{
	public class LinkProperty : PropertyData
	{
		private ContentItem _linkedItem;

		#region Constuctors

		public LinkProperty()
		{
		}

		public LinkProperty(ContentItem containerItem, string name, ContentItem value)
			: base(containerItem, name)
		{
			LinkedItem = value;
		}

		#endregion

		public virtual ContentItem LinkedItem
		{
			get { return _linkedItem; }
			set
			{
				_linkedItem = value;
				if (value != null)
					LinkValue = value.ID;
				else
					LinkValue = null;
			}
		}

		internal virtual int? LinkValue { get; set; }

		public override PropertyDataType Type
		{
			get { return PropertyDataType.Link; }
		}

		public override object Value
		{
			get { return LinkedItem; }
			set { LinkedItem = (ContentItem) value; }
		}

		public override Type ValueType
		{
			get { return typeof(ContentItem); }
		}

		public override IEditor GetDefaultEditor(string title, int sortOrder, Type propertyType)
		{
			return new LinkedItemDropDownListEditor(title, sortOrder) { TypeFilter = propertyType };
		}
	}
}