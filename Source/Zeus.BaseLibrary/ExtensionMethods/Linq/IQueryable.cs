using System;
using System.Linq;
using System.Linq.Expressions;

namespace Zeus.BaseLibrary.ExtensionMethods.Linq
{
	public static class IQueryableExtensionMethods
	{
		public static IQueryable OfType(this IQueryable source, Type resultType)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery(
				Expression.Call(null, (typeof(Queryable).GetMethod("OfType")).MakeGenericMethod(resultType), source.Expression));
		}

		public static IQueryable<TSource> OfType<TSource>(this IQueryable<TSource> source, Type type)
		{
			return ((IQueryable) source).OfType(type).Cast<TSource>();
		}
	}
}