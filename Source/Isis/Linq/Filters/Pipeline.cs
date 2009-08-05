using System;
using System.Linq;
using System.Collections.Generic;

namespace Isis.Linq.Filters
{
	public class Pipeline<T> : IFilter<T>
	{
		public IList<IFilter<T>> Filters
		{
			get;
			protected set;
		}

		public Pipeline()
		{
			this.Filters = new List<IFilter<T>>();
		}

		public IQueryable<T> Filter(IQueryable<T> source)
		{
			var filteredValues = source;
			foreach (var filter in this.Filters)
				filteredValues = filter.Filter(filteredValues);
			return filteredValues;
		}
	}
}
