using System;
using System.Linq;

namespace Zeus.Linq
{
	public interface IItemFinder : IOrderedQueryable<ContentItem>
	{
		IOrderedQueryable<T> Elements<T>()
			where T : ContentItem;
	}
}
