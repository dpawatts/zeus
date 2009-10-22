using System.Collections.Generic;
using System;
using System.Linq;

namespace Zeus.BaseLibrary.ExtensionMethods.Collections.Generic
{
	public static class IListExtensionMethods
	{
		public static bool RemoveSingleOrDefault<T>(this IList<T> list, Func<T, bool> predicate)
		{
			return list.Remove(list.SingleOrDefault(predicate));
		}

		public static bool RemoveSingle<T>(this IList<T> list, Func<T, bool> predicate)
		{
			return list.Remove(list.Single(predicate));
		}
	}
}