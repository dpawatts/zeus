using System;
using System.Collections.Generic;

namespace Isis.Collections.Generic
{
	/// <summary>
	/// Copied from http://code.google.com/p/mb-unit/source/browse/trunk/v3/src/MbUnit/MbUnit/Framework/MatchTable.cs
	/// Helper class used to compare two sequences of unordered elements.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal class MatchTable<T>
	{
		private readonly EqualityComparison<T> comparer;
		private readonly List<KeyValuePair<T, Pair<int, int>>> items;
		private int nonEqualCount;

		public MatchTable(EqualityComparison<T> comparer)
		{
			this.comparer = comparer;
			items = new List<KeyValuePair<T, Pair<int, int>>>();
		}

		public int NonEqualCount
		{
			get
			{
				return nonEqualCount;
			}
		}

		public IEnumerable<KeyValuePair<T, Pair<int, int>>> Items
		{
			get
			{
				return items;
			}
		}

		public void AddLeftValue(T key)
		{
			Add(key, 1, 0);
		}

		public void AddRightValue(T key)
		{
			Add(key, 0, 1);
		}

		private void Add(T key, int expectedCount, int actualCount)
		{
			for (int i = 0; i < items.Count; i++)
			{
				KeyValuePair<T, Pair<int, int>> item = items[i];
				if (comparer(item.Key, key))
				{
					Pair<int, int> oldCounters = items[i].Value;
					Pair<int, int> newCounters = new Pair<int, int>(oldCounters.First + expectedCount, oldCounters.Second + actualCount);
					items[i] = new KeyValuePair<T, Pair<int, int>>(item.Key, newCounters);

					if (newCounters.First == newCounters.Second)
						nonEqualCount -= 1;
					else if (oldCounters.First == oldCounters.Second)
						nonEqualCount += 1;

					return;
				}
			}

			items.Add(new KeyValuePair<T, Pair<int, int>>(key, new Pair<int, int>(expectedCount, actualCount)));
			nonEqualCount += 1;
		}
	}

	/// <summary>
	/// Copied from http://code.google.com/p/mb-unit/source/browse/trunk/v3/src/Gallio/Gallio/Common/Pair.cs
	/// An immutable record that holds two values.
	/// </summary>
	/// <typeparam name="TFirst">The type of the first value.</typeparam>
	/// <typeparam name="TSecond">The type of the second value.</typeparam>
	public struct Pair<TFirst, TSecond>
	{
		private readonly TFirst first;
		private readonly TSecond second;

		/// <summary>
		/// Creates a pair.
		/// </summary>
		/// <param name="first">The first value.</param>
		/// <param name="second">The second value.</param>
		public Pair(TFirst first, TSecond second)
		{
			this.first = first;
			this.second = second;
		}

		/// <summary>
		/// Gets the first value.
		/// </summary>
		public TFirst First
		{
			get { return first; }
		}

		/// <summary>
		/// Gets the second value.
		/// </summary>
		public TSecond Second
		{
			get { return second; }
		}
	}

	/// <summary>
	/// Copied from http://code.google.com/p/mb-unit/source/browse/trunk/v3/src/Gallio/Gallio/Common/EqualityComparison.cs
	/// Represents the method that determines whether two objects of the same type are equal.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This delegate is to <see cref="IEquatable{T}"/>, what <see cref="Comparison{T}"/> is
	/// to <see cref="IComparable{T}"/>.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">The type of the objects to compare.</typeparam>
	/// <param name="x">The first object to compare.</param>
	/// <param name="y">The second object to compare.</param>
	/// <returns>True if the object are equal; otherwise false.</returns>
	public delegate bool EqualityComparison<T>(T x, T y);
}