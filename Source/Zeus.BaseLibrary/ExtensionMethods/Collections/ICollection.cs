using System.Collections;

namespace Zeus.BaseLibrary.ExtensionMethods.Collections
{
	public static class ICollectionExtensionMethods
	{
		public static bool IsNullOrEmpty(this ICollection collection)
		{
			return (collection == null || collection.Count == 0);
		}
	}
}