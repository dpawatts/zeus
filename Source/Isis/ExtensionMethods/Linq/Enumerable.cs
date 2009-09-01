using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Text;

namespace Isis.ExtensionMethods.Linq
{
	public static class EnumerableExtensionMethods
	{
		#region Distinct 1

		public static IEnumerable<TSource> Distinct<TSource, TResult>(
				this IEnumerable<TSource> source, Func<TSource, TResult> comparer)
		{
			return source.Distinct(new DynamicComparer<TSource, TResult>(comparer));
		}

		private class DynamicComparer<T, TResult> : IEqualityComparer<T>
		{
			private readonly Func<T, TResult> _selector;

			public DynamicComparer(Func<T, TResult> selector)
			{
				_selector = selector;
			}

			public bool Equals(T x, T y)
			{
				TResult result1 = _selector(x);
				TResult result2 = _selector(y);
				return result1.Equals(result2);
			}

			public int GetHashCode(T obj)
			{
				TResult result = _selector(obj);
				return result.GetHashCode();
			}
		}

		#endregion

		#region Distinct 2

		private class EqualityComparer<T> : IEqualityComparer<T>
		{
			public Func<T, T, bool> Comparer { get; internal set; }
			public Func<T, int> Hasher { get; internal set; }

			bool IEqualityComparer<T>.Equals(T x, T y)
			{
				return this.Comparer(x, y);
			}

			int IEqualityComparer<T>.GetHashCode(T obj)
			{
				// No hashing capabilities. Default to Equals(x, y).
				if (this.Hasher == null)
					return 0;

				return this.Hasher(obj);
			}
		}

		/// <summary>
		/// Gets distinct items by a comparer delegate.
		/// </summary>
		public static IEnumerable<T> Distinct<T>(this IEnumerable<T> enumeration, Func<T, T, bool> comparer)
		{
			return Distinct(enumeration, comparer, null);
		}

		/// <summary>
		/// Gets distinct items by comparer and hasher delegates (faster than only comparer).
		/// </summary>
		public static IEnumerable<T> Distinct<T>(this IEnumerable<T> enumeration, Func<T, T, bool> comparer, Func<T, int> hasher)
		{
			// Check to see that enumeration is not null
			if (enumeration == null)
				throw new ArgumentNullException("enumeration");

			// Check to see that comparer is not null
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return enumeration.Distinct(new EqualityComparer<T> { Comparer = comparer, Hasher = hasher });
		}

		#endregion

		public static bool Contains<TSource, TResult>(
				this IEnumerable<TSource> source, TResult value, Func<TSource, TResult> selector)
		{
			foreach (TSource sourceItem in source)
			{
				TResult sourceValue = selector(sourceItem);
				if (sourceValue.Equals(value))
					return true;
			}
			return false;
		}

		public static DataTable ToDataTable(this IEnumerable enumeration)
		{
			DataTable dataTable = new DataTable();

			// Base the properties on the first item in the list.
			object value = enumeration.Cast<object>().FirstOrDefault();
			if (value != null)
			{
				PropertyInfo[] properties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

				// Create the columns in the DataTable.
				foreach (PropertyInfo pi in properties)
				{
					Type underlyingType = Nullable.GetUnderlyingType(pi.PropertyType);
					Type columnType = underlyingType ?? pi.PropertyType;
					dataTable.Columns.Add(pi.Name, columnType);
				}

				// Populate the table.
				foreach (object item in enumeration)
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow.BeginEdit();
					foreach (PropertyInfo pi in properties)
						if (pi.GetIndexParameters() == null || pi.GetIndexParameters().Length == 0) // exclude indexers
						{
							object propertyValue = pi.GetValue(item, null);
							dataRow[pi.Name] = propertyValue ?? DBNull.Value;
						}
					dataRow.EndEdit();
					dataTable.Rows.Add(dataRow);
				}
			}

			return dataTable;
		}

		public static string Join(this IEnumerable<string> source, string separator)
		{
			return string.Join(separator, source.ToArray());
		}

		public static string Join(this IEnumerable<string> source, string separator, string prefix, string suffix)
		{
			string[] values = source.ToArray();
			for (int i = 0, length = values.Length; i < length; i++)
				values[i] = prefix + values[i] + suffix;
			return string.Join(separator, values);
		}

		public static string Join(this IEnumerable<string> source, string separator, string format)
		{
			string[] values = source.ToArray();
			for (int i = 0, length = values.Length; i < length; i++)
				values[i] = string.Format(format, values[i]);
			return string.Join(separator, values);
		}

		public static string Join<T>(this IEnumerable<T> source, Func<T, string> valueCallback, string separator)
		{
			T[] values = source.ToArray();
			StringBuilder sb = new StringBuilder();
			for (int i = 0, length = values.Length; i < length; i++)
			{
				sb.Append(valueCallback(values[i]));
				if (i < length - 1)
					sb.Append(separator);
			}
			return sb.ToString();
		}

		public static IEnumerable<T> OfType<T>(this IEnumerable<T> source, Type type)
		{
			foreach (T element in source)
				if (element != null && type.IsAssignableFrom(element.GetType()))
					yield return element;
		}
	}
}
