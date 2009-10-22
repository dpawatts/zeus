using System.Collections.Generic;

namespace Zeus.BaseLibrary.Collections.Generic
{
	public static class PageableExtensions
	{
		public static IPageable<T> AsPageable<T>(this IEnumerable<T> items, bool paged, int pageNumber, int pageSize)
		{
			return new Pageable<T>(items, paged, pageNumber, pageSize);
		}
	}
}