using System.Linq.Expressions;
using System;

namespace Zeus.Persistence.Specifications
{
	/// <summary>
	/// The <see cref="ISpecification{T}"/> interface defines a basic contract to
	/// express specifications declaratively and allow to be used by a <see cref="IItemFinder{T}"/>
	/// instance.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISpecification<T>
	{
		/// <summary>
		/// Gets a boolean which indicates whether the specification needs to be executed in-memory.
		/// This is used for expressions which the LINQ provider cannot deal with, such as local method calls.
		/// </summary>
		bool InMemory { get; }

		/// <summary>
		/// Gets the expression that encapsulates the criteria of the specification.
		/// </summary>
		Expression<Func<T, bool>> Predicate { get; }

		/// <summary>
		/// Evaluates the specification against an entity of <typeparamref name="T"/>.
		/// </summary>
		/// <param name="entity">The <typeparamref name="T"/> instance to evaulate the specificaton
		/// against.</param>
		/// <returns>Should return true if the specification was satisfied by the entity, else false. </returns>
		bool IsSatisfiedBy(T entity);
	}
}