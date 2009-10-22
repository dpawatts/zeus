using System;

namespace Zeus.BaseLibrary.ExtensionMethods
{
	public static class Int32ExtensionMethods
	{
		public static string ToOrdinal(this int number)
		{
			const string TH = "th";
			var s = number.ToString();

			number %= 100;

			if ((number >= 11) && (number <= 13))
				return s + TH;

			switch (number % 10)
			{
				case 1:
					return s + "st";
				case 2:
					return s + "nd";
				case 3:
					return s + "rd";
				default:
					return s + TH;
			}
		}

		public static int Clamp(this int value, int min, int max)
		{
			if (value < min)
				return min;
			if (value > max)
				return max;
			return value;
		}

		public static string ToMonthName(this int value)
		{
			return new DateTime(DateTime.Today.Year, value, DateTime.Today.Day).ToString("MMMM");
		}
	}
}