using System;
using System.Linq;

namespace Zeus.Linq.Filters
{
	public class VisibleFilter : ItemFilter
	{
		public override IQueryable<ContentItem> Filter(IQueryable<ContentItem> source)
		{
			return source.Where(ci => ci.Visible);
		}
	}
}
