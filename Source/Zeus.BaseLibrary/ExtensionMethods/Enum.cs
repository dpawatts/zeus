using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using Isis.ExtensionMethods.Reflection;

namespace Isis.ExtensionMethods
{
	public static class EnumExtensionMethods
	{
		public static string GetDescription(this Enum value)
		{
			return EnumHelper.GetEnumValueDescription(value.GetType(), value.ToString());
		}

		public static bool EqualsAny(this Enum value, params Enum[] valuesToCompare)
		{
			foreach (Enum enumValue in valuesToCompare)
				if (value.Equals(enumValue))
					return true;
			return false;
		}
	}
}