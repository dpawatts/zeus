using System;
using System.Collections;
using System.Reflection;
using System.Web.Mvc;

namespace Zeus.Web.Mvc
{
	internal class ViewDataDictionaryWrapper : IDictionary
	{
		private ViewDataDictionary _innerViewData;
		private IDictionary _innerDictionary;

		public ViewDataDictionaryWrapper(ViewDataDictionary viewData)
		{
			_innerViewData = viewData;
		}

		private IDictionary InnerDictionary
		{
			get
			{
				if (_innerDictionary == null)
				{
					// TODO: Nasty, but need to access IDictionary.
					FieldInfo field = _innerViewData.GetType().GetField("_innerDictionary", BindingFlags.NonPublic | BindingFlags.Instance);
					_innerDictionary = field.GetValue(_innerViewData) as IDictionary;
				}
				return _innerDictionary;
			}
		}

		#region Implementation of IEnumerable

		public bool Contains(object key)
		{
			return InnerDictionary.Contains(key);
		}

		public void Add(object key, object value)
		{
			InnerDictionary.Add(key, value);
		}

		public void Clear()
		{
			InnerDictionary.Clear();
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return InnerDictionary.GetEnumerator();
		}

		public void Remove(object key)
		{
			InnerDictionary.Remove(key);
		}

		public object this[object key]
		{
			get { return InnerDictionary[key]; }
			set { InnerDictionary[key] = value; }
		}

		public ICollection Keys
		{
			get { return InnerDictionary.Keys; }
		}

		public ICollection Values
		{
			get { return InnerDictionary.Values; }
		}

		public bool IsReadOnly
		{
			get { return InnerDictionary.IsReadOnly; }
		}

		public bool IsFixedSize
		{
			get { return InnerDictionary.IsFixedSize; }
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Implementation of ICollection

		public void CopyTo(Array array, int index)
		{
			InnerDictionary.CopyTo(array, index);
		}

		public int Count
		{
			get { return InnerDictionary.Count; }
		}

		public object SyncRoot
		{
			get { return InnerDictionary.SyncRoot; }
		}

		public bool IsSynchronized
		{
			get { return InnerDictionary.IsSynchronized; }
		}

		#endregion
	}
}