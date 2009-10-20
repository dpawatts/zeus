using System;
using System.Linq;

namespace Isis.Linq.Filters
{
	public interface IFilter<T>
	{
		IQueryable<T> Filter(IQueryable<T> source);
	}
}
