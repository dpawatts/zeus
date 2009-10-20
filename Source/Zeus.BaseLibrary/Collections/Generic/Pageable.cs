using System;
using System.Collections.Generic;
using MvcContrib.Pagination;

namespace Isis.Collections.Generic
{
	public class Pageable<T> : IPageable<T>
	{
		private readonly IEnumerable<T> _nonPagedItems;
		private readonly IPagination<T> _pagedItems;

		public Pageable(IEnumerable<T> items, bool paged, int pageNumber, int pageSize)
		{
			_nonPagedItems = items;
			if (paged)
				_pagedItems = items.AsPagination(pageNumber, pageSize);
			Paged = paged;
		}

		public bool Paged { get; private set; }

		public IEnumerable<T> NonPagedItems
		{
			get { return _nonPagedItems; }
		}

		public IPagination<T> PagedItems
		{
			get
			{
				if (!Paged)
					throw new InvalidOperationException("Cannot call PagedItems when Paged is set to false.");
				return _pagedItems;
			}
		}

		private IEnumerable<T> ActiveItems
		{
			get { return (Paged) ? PagedItems : NonPagedItems; }
		}

		#region IEnumerable<T> Members

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return ActiveItems.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ActiveItems.GetEnumerator();
		}

		#endregion
	}
}