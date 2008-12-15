using System;
using System.Web;

namespace Zeus.Web
{
	public class Url
	{
		private string _url;
		private string _query;

		public Url(string url)
		{
			_url = url;
		}

		public static string ApplicationPath
		{
			get { return System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath; }
		}

		public Url AppendQuery(string key, int value)
		{
			string keyValue = key + "=" + value;
			return AppendQuery(keyValue);
		}

		public Url AppendQuery(string keyValue)
		{
			if (string.IsNullOrEmpty(_query))
				_query = keyValue;
			else
				_query += "&" + keyValue;
			return this;
		}

		public static Url Parse(string url)
		{
			if (url == null)
				return null;
			else if (url.StartsWith("~"))
				url = ToAbsolute(url);

			return new Url(url);
		}

		public static string ToAbsolute(string url)
		{
			if (!string.IsNullOrEmpty(url) && url[0] == '~' && url.Length > 2)
				url = ApplicationPath + url.Substring(2);
			else if (url == "~")
				url = ApplicationPath;

			return url;
		}

		public override string ToString()
		{
			return _url + "?" + _query;
		}

		public static string PathPart(string url)
		{
			url = RemoveHash(url);

			int queryIndex = QueryIndex(url);
			if (queryIndex >= 0)
				url = url.Substring(0, queryIndex);

			return url;
		}

		private static int QueryIndex(string url)
		{
			return url.IndexOf('?');
		}

		/// <summary>Removes the hash (#...) from an url.</summary>
		/// <param name="url">An url that might hav a hash in it.</param>
		/// <returns>An url without the hash part.</returns>
		public static string RemoveHash(string url)
		{
			int hashIndex = url.IndexOf('#');
			if (hashIndex >= 0)
				url = url.Substring(0, hashIndex);
			return url;
		}
	}
}
