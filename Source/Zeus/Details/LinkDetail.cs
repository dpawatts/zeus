using System;

namespace Zeus.Details
{
	public class LinkDetail : ContentDetail
	{
		#region Constructors

		public LinkDetail()
		{
		}

		public LinkDetail(ContentItem containerItem, string name, ContentItem value)
		{
			ID = 0;
			EnclosingItem = containerItem;
			Name = name;
			LinkedItem = value;
		}

		#endregion

		private ContentItem _linkedItem;

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

		protected virtual int? LinkValue
		{
			get;
			set;
		}

		public override object Value
		{
			get { return this.LinkedItem; }
			set { this.LinkedItem = (ContentItem) value; }
		}

		public override Type ValueType
		{
			get { return typeof(ContentItem); }
		}
	}
}
