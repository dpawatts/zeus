using System.Collections.Generic;

namespace Zeus.Collections
{
	/// <summary>
	/// An item comparer. This class can compare classes given a expression.
	/// </summary>
	/// <typeparam name="T">
	/// The type of items to compare.
	/// </typeparam>
	public class ItemComparer<T> : IComparer<T>
		where T : ContentItem
	{
		#region Constructors

		/// <summary>Creates a new instance of the ItemComparer that sorts on the item's sort order.</summary>
		public ItemComparer()
			: this("SortOrder")
		{
		}

		/// <summary>Creates a new instance of the ItemComparer that sorts using a custom sort expression.</summary>
		/// <param name="sortExpression">The name of the property to sort on. DESC can be appended to the string to reverse the sort order.</param>
		public ItemComparer(string sortExpression)
		{
			string[] pair = sortExpression.Split(' ');
			DetailToCompare = pair[0];
			if (pair.Length > 1 && string.Compare(pair[1], "DESC", true) == 0)
				Inverse = true;
		}

		/// <summary>Creates a new instance of the ItemComparer that sorts using sort property and direction.</summary>
		/// <param name="detailToCompare">The name of the property to sort on.</param>
		/// <param name="inverse">Wether the comparison should be "inverse", i.e. make Z less than A.</param>
		public ItemComparer(string detailToCompare, bool inverse)
		{
			DetailToCompare = detailToCompare;
			Inverse = inverse;
		}

		#endregion

		#region Static Methods

		/// <summary>Compares two items with each other.</summary>
		/// <param name="x">The first item.</param>
		/// <param name="y">The second item</param>
		/// <param name="detailToCompare">The detail name to use for comparison.</param>
		/// <param name="inverse">Inverse the comparison.</param>
		/// <returns>The compared difference.</returns>
		public static int Compare(T x, T y, string detailToCompare, bool inverse)
		{
			object ox = x[detailToCompare];
			object oy = y[detailToCompare];
			if (inverse)
				return System.Collections.Comparer.Default.Compare(oy, ox);
			else
				return System.Collections.Comparer.Default.Compare(ox, oy);
		}

		#endregion

		#region IComparer & IComparer<T> Members

		/// <summary>Compares to items.</summary>
		/// <param name="x">The first item.</param>
		/// <param name="y">The second item.</param>
		/// <returns>The comparison result.</returns>
		public int Compare(object x, object y)
		{
			if (x is T && y is T)
				return Compare((T) x, (T) y);
			return 0;
		}

		/// <summary>Compares two items.</summary>
		/// <param name="x">The first item.</param>
		/// <param name="y">The second item.</param>
		/// <returns>The comparison result.</returns>
		public int Compare(T x, T y)
		{
			return Compare(x, y, DetailToCompare, Inverse);
		}

		/// <summary>Gets or sets the detail name to use for comparison.</summary>
		public string DetailToCompare { get; set; }

		/// <summary>Gets or sets wether the comparison should be inversed.</summary>
		public bool Inverse { get; set; }

		#endregion
	}
}