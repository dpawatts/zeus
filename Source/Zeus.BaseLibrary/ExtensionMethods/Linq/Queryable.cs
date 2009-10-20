using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Isis.ExtensionMethods.Linq
{
	public static class QueryableExtensionMethods
	{
		public static IQueryable OfType(this IQueryable source, Type resultType)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery(
				Expression.Call(null, (typeof(Queryable).GetMethod("OfType")).MakeGenericMethod(resultType), source.Expression));
		}
	}
}
