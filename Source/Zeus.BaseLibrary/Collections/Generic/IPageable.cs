using System.Collections.Generic;
using MvcContrib.Pagination;

namespace Zeus.BaseLibrary.Collections.Generic
{
	public interface IPageable<T> : IEnumerable<T>
	{
		bool Paged { get; }
		IPagination<T> PagedItems { get; }
	}
}