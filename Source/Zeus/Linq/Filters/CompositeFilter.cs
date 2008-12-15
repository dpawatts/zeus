using System;
using System.Linq;

namespace Zeus.Linq.Filters
{
	public class CompositeFilter : ItemFilter
	{
		private ItemFilter[] _filters;

		public CompositeFilter(params ItemFilter[] filters)
		{
			_filters = filters;
		}

		public override IQueryable<ContentItem> Filter(IQueryable<ContentItem> source)
		{
			IQueryable<ContentItem> result = source;
			foreach (ItemFilter filter in _filters)
				result = filter.Filter(result);
			return result;
		}
	}
}
