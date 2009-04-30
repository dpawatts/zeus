using System;
using System.Linq;
using System.Linq.Expressions;

namespace Zeus.Persistence.Specifications
{
	/// <summary>
	/// Provides a default implementation of the <see cref="Specification{T}"/> interface.
	/// </summary>
	/// <remarks>
	/// The <see cref="ISpecification{T}"/> implements Composite Specification pattern by overloading
	/// the & and | (And, Or in VB.Net) operators to allow composing multiple specifications together.
	/// </remarks>
	public class Specification<T> : ISpecification<T>
	{
		#region Fields

		private readonly Expression<Func<T, bool>> _predicate;

		#endregion

		#region Constructor

		/// <summary>
		/// Default Constructor.
		/// Creates a new instance of the <see cref="Specification{T}"/> instnace with the
		/// provided predicate expression.
		/// </summary>
		/// <param name="predicate">A predicate that can be used to check entities that
		/// satisfy the specification.</param>
		public Specification(Expression<Func<T, bool>> predicate)
		{
			_predicate = predicate;
		}

		#endregion

		#region Implementation of ISpecification<T>

		/// <summary>
		/// Gets a boolean which indicates whether the specification needs to be executed in-memory.
		/// This is used for expressions which the LINQ provider cannot deal with, such as local method calls.
		/// </summary>
		public virtual bool InMemory
		{
			get { return false; }
		}

		/// <summary>
		/// Gets the expression that encapsulates the criteria of the specification.
		/// </summary>
		public Expression<Func<T, bool>> Predicate
		{
			get { return _predicate; }
		}

		/// <summary>
		/// Evaluates the specification against an entity of <typeparamref name="T"/>.
		/// </summary>
		/// <param name="entity">The <typeparamref name="T"/> instance to evaulate the specificaton
		/// against.</param>
		/// <returns>Should return true if the specification was satisfied by the entity, else false. </returns>
		public bool IsSatisfiedBy(T entity)
		{
			return _predicate.Compile().Invoke(entity);
		}

		#endregion

		/// <summary>
		/// Overloads the & operator and combines two <see cref="Specification{T}"/> in a Boolean And expression
		/// and returns a new see cref="Specification{T}"/>.
		/// </summary>
		/// <param name="leftHand">The left hand <see cref="Specification{T}"/> to combine.</param>
		/// <param name="rightHand">The right hand <see cref="Specification{T}"/> to combine.</param>
		/// <returns>The combined <see cref="Specification{T}"/> instance.</returns>
		public static ISpecification<T> And(ISpecification<T> leftHand, ISpecification<T> rightHand)
		{
			var rightInvoke = Expression.Invoke(rightHand.Predicate, leftHand.Predicate.Parameters.Cast<Expression>());
			var newExpression = Expression.MakeBinary(ExpressionType.AndAlso, leftHand.Predicate.Body, rightInvoke);
			return new Specification<T>(Expression.Lambda<Func<T, bool>>(newExpression, leftHand.Predicate.Parameters));
		}

		/// <summary>
		/// Overloads the & operator and combines two <see cref="Specification{T}"/> in a Boolean Or expression
		/// and returns a new see cref="Specification{T}"/>.
		/// </summary>
		/// <param name="leftHand">The left hand <see cref="Specification{T}"/> to combine.</param>
		/// <param name="rightHand">The right hand <see cref="Specification{T}"/> to combine.</param>
		/// <returns>The combined <see cref="Specification{T}"/> instance.</returns>
		public static ISpecification<T> Or(ISpecification<T> leftHand, ISpecification<T> rightHand)
		{
			var rightInvoke = Expression.Invoke(rightHand.Predicate, leftHand.Predicate.Parameters.Cast<Expression>());
			var newExpression = Expression.MakeBinary(ExpressionType.OrElse, leftHand.Predicate.Body, rightInvoke);
			return new Specification<T>(Expression.Lambda<Func<T, bool>>(newExpression, leftHand.Predicate.Parameters));
		}
	}
}