using System;
using System.Linq;

namespace Zeus.Linq.Filters
{
	public class PublishedFilter : ItemFilter
	{
		public override IQueryable<ContentItem> Filter(IQueryable<ContentItem> source)
		{
			return source.Where(ci => (ci.Published.HasValue && ci.Published.Value <= DateTime.Now)
				&& !(ci.Expires.HasValue && ci.Expires.Value < DateTime.Now));
		}
	}
}
