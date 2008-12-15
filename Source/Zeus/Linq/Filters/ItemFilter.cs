using System;
using Isis.Linq.Filters;
using System.Linq;

namespace Zeus.Linq.Filters
{
	public abstract class ItemFilter : IFilter<ContentItem>
	{
		public abstract IQueryable<ContentItem> Filter(IQueryable<ContentItem> source);
	}
}
