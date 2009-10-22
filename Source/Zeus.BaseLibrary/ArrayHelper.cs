using System;

namespace Zeus.BaseLibrary
{
	public static class ArrayHelper
	{
		/// <summary>
		/// Only works if arrays contain only value types or strings
		/// </summary>
		/// <param name="array1"></param>
		/// <param name="array2"></param>
		/// <returns></returns>
		public static bool Equals(Array array1, Array array2)
		{
			if (array1 == null && array2 == null)
				return true;

			if (array1 == null || array2 == null)
				return false;

			if (array1.Length != array2.Length)
				return false;

			for (int i = 0, length = array1.Length; i < length; ++i)
				if (array1.GetValue(i) != array2.GetValue(i))
					return false;

			return true;
		}
	}
}