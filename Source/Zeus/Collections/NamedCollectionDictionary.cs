using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeus.ContentTypes;

namespace Zeus.Collections
{
	public class NamedCollectionDictionary<T> : ICollection<T>, IDictionary<string, T>
		where T : IUniquelyNamed
	{
		private readonly IList<T> _internalList;

		public NamedCollectionDictionary(IList<T> internalList)
		{
			_internalList = internalList;
		}

		#region ICollection members

		IEnumerator<KeyValuePair<string, T>> IEnumerable<KeyValuePair<string, T>>.GetEnumerator()
		{
			return _internalList.ToDictionary(pd => pd.Name).GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(T item)
		{
			_internalList.Add(item);
		}

		public void Add(KeyValuePair<string, T> item)
		{
			Add(item.Key, item.Value);
		}

		public void Clear()
		{
			_internalList.Clear();
		}

		public bool Contains(KeyValuePair<string, T> item)
		{
			return ContainsKey(item.Key);
		}

		public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public bool Remove(KeyValuePair<string, T> item)
		{
			return Remove(item.Key);
		}

		public bool Contains(T item)
		{
			return _internalList.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_internalList.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			return _internalList.Remove(item);
		}

		public int Count
		{
			get { return _internalList.Count; }
		}

		public bool IsReadOnly
		{
			get { return _internalList.IsReadOnly; }
		}

		#endregion

		#region Implementation of IDictionary<string,T>

		public bool ContainsKey(string key)
		{
			return _internalList.Any(pd => pd.Name == key);
		}

		public void Add(string key, T value)
		{
			if (ContainsKey(key))
				throw new ArgumentException("An element with the key '" + key + "' already exists.");

			_internalList.Add(value);
		}

		public bool Remove(string key)
		{
			return _internalList.Remove(_internalList.Single(pd => pd.Name == key));
		}

		public bool TryGetValue(string key, out T value)
		{
			if (ContainsKey(key))
			{
				value = this[key];
				return true;
			}

			value = default(T);
			return false;
		}

		public T this[string key]
		{
			get
			{
				if (!ContainsKey(key))
					throw new KeyNotFoundException("The key '" + key + "' does not exist.");

				return _internalList.Single(pd => pd.Name == key);
			}
			set
			{
				if (ContainsKey(key))
				{
					int index = _internalList.IndexOf(_internalList.Single(pd => pd.Name == key));
					_internalList[index] = value;
				}
				else
				{
					_internalList.Add(value);
				}
			}
		}

		public ICollection<string> Keys
		{
			get { return _internalList.ToDictionary(pd => pd.Name).Keys; }
		}

		public ICollection<T> Values
		{
			get { return _internalList.ToDictionary(pd => pd.Name).Values; }
		}

		#endregion
	}
}