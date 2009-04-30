using System;
using System.Collections.Specialized;
using System.Web;

namespace Zeus.Globalization
{
	public class LanguageSelection
	{
		public static bool IsCandidateMatch(string languageCode1, string languageCode2)
		{
			int index = languageCode1.IndexOf('-');
			int length = languageCode2.IndexOf('-');
			if (index < 0)
				index = languageCode1.Length;
			if (length < 0)
				length = languageCode2.Length;
			if (index != length)
				return false;
			if (languageCode2.Equals("no", StringComparison.OrdinalIgnoreCase))
				return (languageCode1.IndexOf(languageCode2, StringComparison.OrdinalIgnoreCase) >= 0);
			return (string.Compare(languageCode1, 0, languageCode2, 0, index, StringComparison.OrdinalIgnoreCase) == 0);
		}
	}
}