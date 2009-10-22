using System.Collections.Generic;
using System.Linq;

namespace Zeus.BaseLibrary.Collections.Generic
{
	public static class CartesianProductUtility
	{
		public static IEnumerable<IEnumerable<T>> Combinations<T>(params IEnumerable<T>[] input)
		{
			IEnumerable<IEnumerable<T>> result = new T[0][];
			foreach (IEnumerable<T> item in input)
				result = Combine(result, Combinations(item));
			return result;
		}

		private static IEnumerable<IEnumerable<T>> Combinations<T>(IEnumerable<T> input)
		{
			foreach (T item in input)
				yield return new [] { item };
		}

		public static IEnumerable<IEnumerable<T>> Combine<T>(IEnumerable<IEnumerable<T>> a, IEnumerable<IEnumerable<T>> b)
		{
			bool found = false;
			foreach (IEnumerable<T> groupa in a)
			{
				found = true;
				foreach (IEnumerable<T> groupB in b)
					yield return groupa.Concat(groupB);
			}
			if (!found)
				foreach (IEnumerable<T> groupB in b)
					yield return groupB;
		}
	}
}