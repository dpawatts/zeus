﻿namespace Protx.Vsp
{
	internal class MyString
	{
		public static bool IsNullOrEmpty(string value)
		{
			if (value != null)
			{
				return (value.Length == 0);
			}
			return true;
		}
	}
}