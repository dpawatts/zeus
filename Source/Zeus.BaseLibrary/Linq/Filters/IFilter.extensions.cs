using System;
using System.Linq;

namespace Isis.Linq.Filters
{
	public static class FilterExtensionMethods
	{
		public static IQueryable<T> WhichAre<T>(this IQueryable<T> source, IFilter<T> criteria)
		{
			return criteria.Filter(source);
		}
	}
}
