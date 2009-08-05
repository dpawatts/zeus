using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Isis.Linq
{
	public static class ExtendedQueryable
	{
		public static IQueryable<TSource> OfType<TSource>(this IQueryable<TSource> source, Type type)
		{
			MethodInfo ofTypeMethod = typeof(Queryable).GetMethod("OfType");
			Expression expression = Expression.Call(null,
				ofTypeMethod.MakeGenericMethod(new [] { type }),
				new [] { source.Expression });
			return source.Provider.CreateQuery(expression).Cast<TSource>();
		}

		public static IQueryable OfType(this IQueryable source, Type type)
		{
			MethodInfo ofTypeMethod = typeof(Queryable).GetMethod("OfType");
			Expression expression = Expression.Call(null,
				ofTypeMethod.MakeGenericMethod(new [] { type }),
				new [] { source.Expression });
			return source.Provider.CreateQuery(expression);
		}
	}
}
