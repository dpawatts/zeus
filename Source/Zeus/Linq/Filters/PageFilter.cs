using System;
using System.Collections.Generic;
using System.Linq;

namespace Zeus.Linq.Filters
{
	public class PageFilter : ItemFilter
	{
		public override IQueryable<ContentItem> Filter(IQueryable<ContentItem> source)
		{
			return source.Where(ci => ci.IsPage);
		}
	}
}
