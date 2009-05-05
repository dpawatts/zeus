using System;
using System.Collections.Generic;
using Isis.ExtensionMethods;
using Zeus.Design.Editors;
using Zeus.FileSystem;
using Zeus.FileSystem.Images;

namespace Zeus.ContentProperties
{
	public abstract class PropertyData : ICloneable
	{
		#region Private fields

		private ContentItem _enclosingItem;

		#endregion

		#region Constructors

		protected PropertyData()
		{
			ID = 0;
			Name = string.Empty;
		}

		protected PropertyData(ContentItem containerItem, string name)
		{
			ID = 0;
			EnclosingItem = containerItem;
			Name = name;
		}

		#endregion

		#region Public properties

		public virtual string ColumnName
		{
			get { return Type + "Value"; }
		}

		/// <summary>Gets or sets the detil's primary key.</summary>
		public virtual int ID { get; set; }

		/// <summary>Gets or sets the name of the detail.</summary>
		public virtual string Name { get; set; }

		public abstract PropertyDataType Type { get; }

		/// <summary>Gets or sets this details' value.</summary>
		public abstract object Value { get; set; }

		/// <summary>Gets the type of value associated with this item.</summary>
		public abstract Type ValueType { get; }

		/// <summary>Gets whether this items belongs to an <see cref="DetailCollection"/>.</summary>
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

		/// <summary>Gets or sets the <see cref="Zeus.Details.DetailCollection"/> associated with this detail. This value can be null which means it's a named detail directly on the item.</summary>
		public virtual PropertyCollection EnclosingCollection { get; set; }

		#endregion

		#region Methods

		public abstract IEditor GetDefaultEditor(string title, int sortOrder, Type propertyType);

		public virtual string GetXhtmlValue()
		{
			return (Value != null) ? Value.ToString() : string.Empty;
		}

		#endregion

		#region ICloneable Members

		/// <summary>Creates a cloned object with the id set to 0.</summary>
		/// <returns>A new ContentDetail with the same Name and Value.</returns>
		public virtual PropertyData Clone()
		{
			PropertyData cloned = (PropertyData) Activator.CreateInstance(GetType());
			cloned.ID = 0;
			cloned.Name = Name;
			cloned.Value = Value;
			return cloned;
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		#endregion
	}
}