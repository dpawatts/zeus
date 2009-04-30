using System;
using System.Linq;
using System.Linq.Expressions;
using Isis.ComponentModel;
using Zeus.Persistence.Specifications;

namespace Zeus.Persistence
{
	public interface IFinder<T> : IService
	{
		/// <summary>
		/// Queries the repository based on the provided specification and returns results that
		/// are only satisfied by the specification.
		/// </summary>
		/// <param name="specification">A <see cref="ISpecification{T}"/> instance used to filter results
		/// that only satisfy the specification(s). Specifications be combined.</param>
		/// <returns>A <see cref="IQueryable{T}"/> that can be used to enumerate over the results
		/// of the query.</returns>
		IQueryable<TResult> FindBySpecification<TResult>(ISpecification<TResult> specification)
			where TResult : T;

		/// <summary>
		/// Helper method which internally calls the method above.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="predicate"></param>
		/// <returns></returns>
		IQueryable<TResult> Find<TResult>(Expression<Func<TResult, bool>> predicate)
			where TResult : T;

		IQueryable<TResult> FindAll<TResult>()
			where TResult : T;

		IQueryable FindAll(Type resultType);
	}
}