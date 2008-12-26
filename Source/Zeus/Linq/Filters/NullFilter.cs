using System;
using System.Linq;

namespace Zeus.Linq.Filters
{
	public class NullFilter : ItemFilter
	{
		public override IQueryable<ContentItem> Filter(IQueryable<ContentItem> source)
		{
			return source;
		}
	}
}
