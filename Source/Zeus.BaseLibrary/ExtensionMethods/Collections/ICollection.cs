using System.Collections;

namespace Isis.ExtensionMethods.Collections
{
	public static class ICollectionExtensionMethods
	{
		public static bool IsNullOrEmpty(this ICollection collection)
		{
			return (collection == null || collection.Count == 0);
		}
	}
}
