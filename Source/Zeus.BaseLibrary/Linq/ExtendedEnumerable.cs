using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Zeus.BaseLibrary.Linq
{
	public static class ExtendedEnumerable
	{
		public static IEnumerable<MonthName> AbbreviatedMonthNames()
		{
			return GetMonthNames(CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames);
		}

		public static IEnumerable<MonthName> MonthNames()
		{
			return GetMonthNames(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames);
		}

		private static IEnumerable<MonthName> GetMonthNames(IEnumerable<string> monthNames)
		{
			int index = 1;
			return monthNames.Where(s => !string.IsNullOrEmpty(s)).Select(s => new MonthName { Month = index++, Name = s });
		}

		public class MonthName
		{
			public int Month { get; set; }
			public string Name { get; set; }
		}
	}
}