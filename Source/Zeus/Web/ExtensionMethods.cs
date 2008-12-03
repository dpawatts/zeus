using System;
using System.Web;

namespace Zeus.Web
{
	public static class ExtensionMethods
	{
		public const string AMP = "&";

		public static Uri AppendQuery(this Uri uri, string key, string value)
		{
			return AppendQuery(uri, key + "=" + HttpUtility.UrlEncode(value));
		}

		public static Uri AppendQuery(this Uri uri, string key, int value)
		{
			return AppendQuery(uri, key + "=" + value);
		}

		public static Uri AppendQuery(this Uri uri, string key, object value)
		{
			if (value == null)
				return uri;
			else
				return AppendQuery(uri, key + "=" + value.ToString());
		}

		public static Uri AppendQuery(this Uri uri, string keyValue)
		{
			UriBuilder clone = new UriBuilder(uri);
			if (string.IsNullOrEmpty(clone.Query))
				clone.Query = keyValue;
			else if (!string.IsNullOrEmpty(keyValue))
				clone.Query += AMP + keyValue;
			return clone.Uri;
		}
	}
}
