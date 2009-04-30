using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Zeus.Persistence.Specifications
{
	public class CompositeSpecification<T> : Specification<T>
		where T : ContentItem
	{
		public CompositeSpecification(params ISpecification<T>[] specifications)
			: base(ComposeSpecifications(specifications))
		{

		}

		private static Expression<Func<T, bool>> ComposeSpecifications(ISpecification<T>[] specifications)
		{
			ISpecification<T> composedSpecification = specifications[0];
			for (int i = 1, length = specifications.Length; i < length; i++)
				composedSpecification = And(composedSpecification, specifications[i]);
			return composedSpecification.Predicate;
		}
	}
}
