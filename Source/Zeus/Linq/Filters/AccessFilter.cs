using System;
using System.Linq;

namespace Zeus.Linq.Filters
{
	public class AccessFilter : ItemFilter
	{
		public override IQueryable<ContentItem> Filter(IQueryable<ContentItem> source)
		{
			// TODO
			return source;
		}
	}
}
