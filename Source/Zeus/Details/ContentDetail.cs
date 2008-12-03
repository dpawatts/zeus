using System;

namespace Zeus.Details
{
	public abstract class ContentDetail
	{
		#region Private fields

		private ContentItem _enclosingItem;
		private DetailCollection _collection;

		#endregion

		#region Public properties

		/// <summary>Gets or sets the detil's primary key.</summary>
		public virtual int ID
		{
			get;
			set;
		}

		/// <summary>Gets or sets the name of the detail.</summary>
		public virtual string Name
		{
			get;
			set;
		}

		/// <summary>Gets or sets this details' value.</summary>
		public abstract object Value
		{
			get;
			set;
		}

		/// <summary>Gets the type of value associated with this item.</summary>
		public abstract Type ValueType
		{
			get;
		}

		/// <summary>Gets whether this items belongs to an <see cref="N2.Details.DetailCollection"/>.</summary>
		public virtual bool IsInCollection
		{
			get { return EnclosingCollection != null; }
		}

		/// <summary>Gets or sets the content item that this detail belong to.</summary>
		public virtual ContentItem EnclosingItem
		{
			get { return _enclosingItem; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");
				_enclosingItem = value;
			}
		}

		/// <summary>Gets or sets the <see cref="N2.Details.DetailCollection"/> associated with this detail. This value can be null which means it's a named detail directly on the item.</summary>
		public virtual DetailCollection EnclosingCollection
		{
			get { return _collection; }
			set { _collection = value; }
		}

		#endregion

		#region Static Methods

		/// <summary>Creates a new content detail of the appropriated type based on the given value.</summary>
		/// <param name="item">The item that will enclose the new detail.</param>
		/// <param name="name">The name of the detail.</param>
		/// <param name="value">The value of the detail. This will determine what type of content detail will be returned.</param>
		/// <returns>A new content detail whose type depends on the type of value.</returns>
		public static ContentDetail New(ContentItem item, string name, object value)
		{
			if (item == null)
				throw new ArgumentNullException("item");
			if (value == null)
				throw new ArgumentNullException("value");

			Type t = value.GetType();
			if (t == typeof(bool))
				return new BooleanDetail(item, name, (bool) value);
			else if (t == typeof(int))
				return new IntegerDetail(item, name, (int) value);
			else if (t == typeof(double))
				return new DoubleDetail(item, name, (double) value);
			else if (t == typeof(DateTime))
				return new DateTimeDetail(item, name, (DateTime) value);
			else if (t == typeof(string))
				return new StringDetail(item, name, (string) value);
			else if (t.IsSubclassOf(typeof(ContentItem)))
				return new LinkDetail(item, name, (ContentItem) value);
			else
				return new ObjectDetail(item, name, value);
		}

		#endregion
	}
}
