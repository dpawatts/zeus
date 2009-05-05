﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Zeus.ContentProperties
{
	public class PropertyCollection : IEnumerable<object>, IList, ICloneable
	{
		#region Private fields

		private ContentItem _enclosingItem;
		private IList<PropertyData> _details = new List<PropertyData>();

		#endregion

		#region Properties

		/// <summary>Gets or sets the collection's primary key.</summary>
		public virtual int ID { get; set; }

		/// <summary>Gets or sets the name of the collection.</summary>
		public virtual string Name { get; set; }

		/// <summary>Gets or sets the details collection. To access the objects directly you can use e.g. collection[index].</summary>
		public IList<PropertyData> Details
		{
			get { return _details; }
			set { _details = value; }
		}

		/// <summary>Gets or sets the the item containing this collection.</summary>
		public virtual ContentItem EnclosingItem
		{
			get { return _enclosingItem; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");
				_enclosingItem = value;
				foreach (PropertyData detail in Details)
					detail.EnclosingItem = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>Creates a new (uninitialized) instance of the DetailCollection class.</summary>
		public PropertyCollection()
		{
		}

		/// <summary>Crates a new instance of the DetailCollection bound to a content item.</summary>
		/// <param name="item">The content item enclosing this collection.</param>
		/// <param name="name">The name of the collection.</param>
		/// <param name="values">The values of this collection.</param>
		public PropertyCollection(ContentItem item, string name, params object[] values)
		{
			EnclosingItem = item;
			Name = name;
			foreach (object value in values)
				Add(value);
		}

		#endregion

		#region Methods

		private PropertyData GetDetail(object val)
		{
			PropertyData detail;
			if (val is PropertyData)
				detail = (PropertyData) val;
			else
				detail = Context.Current.Resolve<IContentPropertyManager>().CreatePropertyDataObject(val.GetType());
			detail.Name = Name;
			detail.EnclosingItem = EnclosingItem;
			detail.EnclosingCollection = this;
			detail.Value = val;
			return detail;
		}

		public void Replace(IEnumerable values)
		{
			bool[] valuesToKeep = new bool[Count];

			// Add new items and mark items that should be kept.
			foreach (object value in values)
			{
				int i = IndexOf(value);
				if (i < 0)
					Add(value);
				else
					valuesToKeep[i] = true;
			}

			// Remove items that are not present in the supplied collection
			for (int i = valuesToKeep.Length - 1; i >= 0; i--)
			{
				if (!valuesToKeep[i])
					RemoveAt(i);
			}
		}

		#endregion

		#region IList Members

		/// <summary>Gets the index of an object in the collection..</summary>
		/// <param name="value">The value whose index to get.</param>
		/// <returns>The index or -1 if the item isn't in the collection.</returns>
		public int IndexOf(object value)
		{
			for (int i = 0; i < Details.Count; i++)
				if (Details[i] == value || Details[i].Value == value)
					return i;
			return -1;
		}

		/// <summary>Inserts a value in the collection.</summary>
		/// <param name="index">The index to insert into.</param>
		/// <param name="value">The value to insert.</param>
		public void Insert(int index, object value)
		{
			PropertyData detail = GetDetail(value);
			Insert(index, detail);
		}

		/// <summary>Removes a value at the given index.</summary>
		/// <param name="index">The index of the value to remove.</param>
		public void RemoveAt(int index)
		{
			Details.RemoveAt(index);
		}

		/// <summary>Gets or sets a value at the specified index.</summary>
		/// <param name="index">The index of the value.</param>
		/// <returns>The value get or set from the specified index.</returns>
		public object this[int index]
		{
			get { return Details[index].Value; }
			set { Details[index] = GetDetail(value); }
		}

		/// <summary>Gets false.</summary>
		public bool IsFixedSize
		{
			get { return false; }
		}

		#endregion

		#region ICollection Members

		/// <summary>Adds a value to the collection.</summary>
		/// <param name="value">The value to add.</param>
		/// <returns>the index of the added value.</returns>
		public int Add(object value)
		{
			PropertyData detail = GetDetail(value);
			Details.Add(detail);
			return Details.Count - 1;
		}

		/// <summary>Clears the collection.</summary>
		public void Clear()
		{
			Details.Clear();
		}

		/// <summary>Check if the collection contains a value.</summary>
		/// <param name="value">The value to look for.</param>
		/// <returns>True if the collection contains the value.</returns>
		public bool Contains(object value)
		{
			if (value == null)
				return false;

			foreach (PropertyData detail in Details)
				if (value.Equals(detail.Value))
					return true;
			return false;
		}

		/// <summary>Copies the collection to an array.</summary>
		/// <param name="array">The array to copy values to.</param>
		/// <param name="index">The start index to copy from.</param>
		public void CopyTo(Array array, int index)
		{
			for (int i = index; i < array.Length; i++)
				array.SetValue(Details[i], i);
		}

		/// <summary>Gets the number of values in the collection.</summary>
		public int Count
		{
			get { return Details.Count; }
		}

		/// <summary>Gets false.</summary>
		public bool IsReadOnly
		{
			get { return Details.IsReadOnly; }
		}

		/// <summary>Removes a value from the collection.</summary>
		/// <param name="value">The value to remove.</param>
		public void Remove(object value)
		{
			int index = IndexOf(value);
			if (index >= 0)
				RemoveAt(index);
		}

		/// <summary>Gets true.</summary>
		public bool IsSynchronized
		{
			get { return true; }
		}

		/// <summary>Gets null.</summary>
		public object SyncRoot
		{
			get { return null; }
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator<object> GetEnumerator()
		{
			return new DetailCollectionEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DetailCollectionEnumerator(this);
		}

		#endregion

		#region DetailCollectionEnumerator

		private class DetailCollectionEnumerator : IEnumerator<object>
		{
			private readonly PropertyCollection collection;

			public DetailCollectionEnumerator(PropertyCollection collection)
			{
				this.collection = collection;
			}

			#region IEnumerator Members

			private int enumeratorIndex = -1;

			public object Current
			{
				get { return collection[enumeratorIndex]; }
			}

			public bool MoveNext()
			{
				return ++enumeratorIndex < collection.Count;
			}

			public void Reset()
			{
				enumeratorIndex = -1;
			}

			#endregion

			public void Dispose()
			{
				
			}
		}

		#endregion

		#region ICloneable Members

		/// <summary>Clones the collection and </summary>
		/// <returns></returns>
		public PropertyCollection Clone()
		{
			PropertyCollection collection = new PropertyCollection();
			collection.ID = 0;
			collection.Name = Name;
			collection.EnclosingItem = EnclosingItem;
			foreach (PropertyData detail in Details)
			{
				PropertyData cloned = detail.Clone();
				cloned.EnclosingCollection = collection;
				collection.Add(cloned);
			}
			return collection;
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		#endregion
	}
}