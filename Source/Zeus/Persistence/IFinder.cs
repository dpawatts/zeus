using System;
using System.Linq;

namespace Zeus.Persistence
{
	public interface IFinder<T>
	{
		IQueryable<T> Items();

		IQueryable<TResult> Items<TResult>()
			where TResult : T;

		IQueryable Items(Type resultType);
	}
}