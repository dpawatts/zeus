using System.Collections;

namespace Zeus.BaseLibrary.Collections.Generic
{
	/// <summary>
	/// A collection of objects that has been split into pages.
	/// </summary>
	public interface IPagination : IEnumerable
	{
		/// <summary>
		/// The current page number
		/// </summary>
		int PageNumber { get; }
		/// <summary>
		/// The number of items in each page.
		/// </summary>
		int PageSize { get; }
		/// <summary>
		/// The total number of items.
		/// </summary>
		int TotalItems { get; }
		/// <summary>
		/// The total number of pages.
		/// </summary>
		int TotalPages { get; }
		/// <summary>
		/// The index of the first item in the page.
		/// </summary>
		int FirstItem { get; }
		/// <summary>
		/// The index of the last item in the page.
		/// </summary>
		int LastItem { get; }
		/// <summary>
		/// Whether there are pages before the current page.
		/// </summary>
		bool HasPreviousPage { get; }
		/// <summary>
		/// Whether there are pages after the current page.
		/// </summary>
		bool HasNextPage { get; }
	}
}