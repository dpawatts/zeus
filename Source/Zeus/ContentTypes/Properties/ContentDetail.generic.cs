using System;

namespace Zeus.ContentTypes.Properties
{
	public abstract class ContentDetail<T> : ContentDetail
	{
		#region Constructor

		public ContentDetail()
		{

		}

		public ContentDetail(ContentItem containerItem, string name, T value)
		{
			this.ID = 0;
			this.EnclosingItem = containerItem;
			this.Name = name;
			this.TypedValue = value;
		}

		#endregion

		#region Properties

		public virtual T TypedValue
		{
			get;
			set;
		}

		public override object Value
		{
			get { return this.TypedValue; }
			set { this.TypedValue = (T) value; }
		}

		public override Type ValueType
		{
			get { return typeof(T); }
		}

		#endregion
	}
}
