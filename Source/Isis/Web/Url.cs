using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Hosting;

namespace Isis.Web
{
	public class Url
	{
		public const string Amp = "&";
		private static readonly char[] _dotsAndSlashes = new[] { '.', '/' };
		private static readonly string[] _querySplitter = new[] { "&amp;", Amp };
		private static readonly char[] _slashes = new[] { '/' };

		static Url()
		{
			DefaultExtension = ".aspx";
		}

		public Url()
		{
			
		}

		public Url(Uri uri)
			: this()
		{
			Scheme = uri.Scheme;
			Authority = uri.Authority;
			Path = uri.AbsolutePath;
			Query = uri.Query;
			Fragment = uri.Fragment;
		}

		public Url(Url other)
			: this()
		{
			Scheme = other.Scheme;
			Authority = other.Authority;
			Path = other.Path;
			Query = other.Query;
			Fragment = other.Fragment;
		}

		public Url(string scheme, string authority, string path, string query, string fragment)
			: this()
		{
			Scheme = scheme;
			Authority = authority;
			Path = path;
			Query = query;
			Fragment = fragment;
		}

		public Url(string scheme, string authority, string rawUrl)
			: this()
		{
			int queryIndex = QueryIndex(rawUrl);
			int hashIndex = rawUrl.IndexOf('#', queryIndex > 0 ? queryIndex : 0);
			LoadFragment(rawUrl, hashIndex);
			LoadQuery(rawUrl, queryIndex, hashIndex);
			LoadSiteRelativeUrl(rawUrl, queryIndex, hashIndex);
			Scheme = scheme;
			Authority = authority;
		}

		public Url(string url)
			: this()
		{
			if (url == null)
			{
				ClearUrl();
			}
			else
			{
				int queryIndex = QueryIndex(url);
				int hashIndex = url.IndexOf('#', queryIndex > 0 ? queryIndex : 0);
				int authorityIndex = url.IndexOf("://");

				LoadFragment(url, hashIndex);
				LoadQuery(url, queryIndex, hashIndex);
				if (authorityIndex >= 0)
					LoadBasedUrl(url, queryIndex, hashIndex, authorityIndex);
				else
					LoadSiteRelativeUrl(url, queryIndex, hashIndex);
			}
		}

		/// <summary>The extension used for url's.</summary>
		public static string DefaultExtension { get; set; }

		public string Scheme { get; private set; }

		public string Authority { get; private set; }

		public string Path { get; private set; }

		public string Query { get; private set; }

		public string Fragment { get; private set; }

		public string Extension
		{
			get
			{
				int index = Path.LastIndexOfAny(_dotsAndSlashes);

				if (index < 0)
					return null;
				if (Path[index] == '/')
					return null;

				return Path.Substring(index);
			}
		}

		public Url HostUrl
		{
			get { return new Url(Scheme, Authority, string.Empty, null, null); }
		}

		/// <summary>Tells whether the url contains authority information.</summary>
		public bool IsAbsolute
		{
			get { return Authority != null; }
		}

		public Url LocalUrl
		{
			get { return new Url(null, null, Path, Query, Fragment); }
		}

		/// <summary>The combination of the path and the query string, e.g. /path.aspx?key=value.</summary>
		public string PathAndQuery
		{
			get { return string.IsNullOrEmpty(Query) ? Path : Path + "?" + Query; }
		}

		public string PathWithoutExtension
		{
			get { return RemoveExtension(Path); }
		}

		public static string ApplicationPath
		{
			get { return HostingEnvironment.ApplicationVirtualPath; }
		}

		/// <summary>The address to the server where the site is running.</summary>
		public static string ServerUrl
		{
			get; set;
		}

		private void ClearUrl()
		{
			Scheme = null;
			Authority = null;
			Path = string.Empty;
			Query = null;
			Fragment = null;
		}

		private void LoadSiteRelativeUrl(string url, int queryIndex, int hashIndex)
		{
			Scheme = null;
			Authority = null;
			if (queryIndex >= 0)
				Path = url.Substring(0, queryIndex);
			else if (hashIndex >= 0)
				Path = url.Substring(0, hashIndex);
			else if (url.Length > 0)
				Path = url;
			else
				Path = "";
		}

		private void LoadBasedUrl(string url, int queryIndex, int hashIndex, int authorityIndex)
		{
			Scheme = url.Substring(0, authorityIndex);
			int slashIndex = url.IndexOf('/', authorityIndex + 3);
			if (slashIndex > 0)
			{
				Authority = url.Substring(authorityIndex + 3, slashIndex - authorityIndex - 3);
				if (queryIndex >= slashIndex)
					Path = url.Substring(slashIndex, queryIndex - slashIndex);
				else if (hashIndex >= 0)
					Path = url.Substring(slashIndex, hashIndex - slashIndex);
				else
					Path = url.Substring(slashIndex);
			}
			else
			{
				// is this case tolerated?
				Authority = url.Substring(authorityIndex + 3);
				Path = "/";
			}
		}

		private void LoadQuery(string url, int queryIndex, int hashIndex)
		{
			if (hashIndex >= 0 && queryIndex >= 0)
				Query = EmptyToNull(url.Substring(queryIndex + 1, hashIndex - queryIndex - 1));
			else if (queryIndex >= 0)
				Query = EmptyToNull(url.Substring(queryIndex + 1));
			else
				Query = null;
		}

		private void LoadFragment(string url, int hashIndex)
		{
			if (hashIndex >= 0)
				Fragment = EmptyToNull(url.Substring(hashIndex + 1));
			else
				Fragment = null;
		}

		private static string EmptyToNull(string text)
		{
			return string.IsNullOrEmpty(text) ? null : text;
		}

		public Url AppendQuery(string key, string value)
		{
			return AppendQuery(key + "=" + HttpUtility.UrlEncode(value));
		}

		public Url AppendQuery(string key, int value)
		{
			return AppendQuery(key + "=" + value);
		}

		public Url AppendQuery(string key, object value)
		{
			if (value == null)
				return this;

			return AppendQuery(key + "=" + value);
		}

		public Url AppendQuery(string keyValue)
		{
			var clone = new Url(this);
			if (string.IsNullOrEmpty(Query))
				clone.Query = keyValue;
			else if (!string.IsNullOrEmpty(keyValue))
				clone.Query += Amp + keyValue;
			return clone;
		}

		/// <summary>Removes the segment at the specified location.</summary>
		/// <param name="index">The segment index to remove</param>
		/// <returns>An url without the specified segment.</returns>
		public Url RemoveSegment(int index)
		{
			if (string.IsNullOrEmpty(Path) || Path == "/" || index < 0)
				return this;

			if (index == 0)
			{
				int slashIndex = Path.IndexOf('/', 1);
				if (slashIndex < 0)
					return new Url(Scheme, Authority, "/", Query, Fragment);
				return new Url(Scheme, Authority, Path.Substring(slashIndex), Query, Fragment);
			}

			string[] segments = PathWithoutExtension.Split(_slashes, StringSplitOptions.RemoveEmptyEntries);
			if (index >= segments.Length)
				return this;

			if (index == segments.Length - 1)
				return RemoveTrailingSegment();

			string newPath = "/" + string.Join("/", segments, 0, index) + "/" + string.Join("/", segments, index + 1, segments.Length - index - 1) + Extension;
			return new Url(Scheme, Authority, newPath, Query, Fragment);
		}

		/// <summary>Removes the last part from the url segments.</summary>
		/// <returns></returns>
		public Url RemoveTrailingSegment(bool maintainExtension)
		{
			if (string.IsNullOrEmpty(Path) || Path == "/")
				return this;

			string newPath = "/";

			int lastSlashIndex = Path.LastIndexOf('/');
			if (lastSlashIndex == Path.Length - 1)
				lastSlashIndex = Path.TrimEnd(_slashes).LastIndexOf('/');
			if (lastSlashIndex > 0)
				newPath = Path.Substring(0, lastSlashIndex) + (maintainExtension ? Extension : "");

			return new Url(Scheme, Authority, newPath, Query, Fragment);
		}

		/// <summary>Removes the last part from the url segments.</summary>
		public Url RemoveTrailingSegment()
		{
			return RemoveTrailingSegment(true);
		}

		public Url SetAuthority(string authority)
		{
			return new Url(Scheme ?? "http", authority, Path, Query, Fragment);
		}

		public Url SetQueryParameter(string key, int value)
		{
			return SetQueryParameter(key, value.ToString());
		}

		public Url SetQueryParameter(string key, string value)
		{
			if (Query == null)
				return AppendQuery(key, value);

			var clone = new Url(this);
			string[] queries = Query.Split(_querySplitter, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < queries.Length; i++)
			{
				if (queries[i].StartsWith(key + "=", StringComparison.InvariantCultureIgnoreCase))
				{
					if (value != null)
					{
						queries[i] = key + "=" + HttpUtility.UrlEncode(value);
						clone.Query = string.Join(Amp, queries);
						return clone;
					}

					if (queries.Length == 1)
						clone.Query = null;
					else if (Query.Length == 2)
						clone.Query = queries[i == 0 ? 1 : 0];
					else if (i == 0)
						clone.Query = string.Join(Amp, queries, 1, queries.Length - 1);
					else if (i == queries.Length - 1)
						clone.Query = string.Join(Amp, queries, 0, queries.Length - 1);
					else
						clone.Query = string.Join(Amp, queries, 0, i) + Amp +
													string.Join(Amp, queries, i + 1, queries.Length - i - 1);
					return clone;
				}
			}
			return AppendQuery(key, value);
		}

		public Url SetQueryParameter(string keyValue)
		{
			if (Query == null)
				return AppendQuery(keyValue);

			int eqIndex = keyValue.IndexOf('=');
			if (eqIndex >= 0)
				return SetQueryParameter(keyValue.Substring(0, eqIndex), keyValue.Substring(eqIndex + 1));

			return SetQueryParameter(keyValue, string.Empty);
		}

		public Url SetScheme(string scheme)
		{
			return new Url(scheme, Authority, Path, Query, Fragment);
		}

		public Url AppendSegment(string segment)
		{
			if (string.IsNullOrEmpty(Path) || Path == "/")
				return AppendSegment(segment, ".aspx");

			return AppendSegment(segment, Extension);
		}

		public Url AppendSegment(string segment, string extension)
		{
			string newPath;
			if (string.IsNullOrEmpty(Path) || Path == "/")
				newPath = "/" + segment + extension;
			else if (!string.IsNullOrEmpty(extension))
			{
				int extensionIndex = Path.LastIndexOf(extension);
				if (extensionIndex >= 0)
					newPath = Path.Insert(extensionIndex, "/" + segment);
				else if (Path.EndsWith("/"))
					newPath = Path + segment + extension;
				else
					newPath = Path + "/" + segment + extension;
			}
			else if (Path.EndsWith("/"))
				newPath = Path + segment;
			else
				newPath = Path + "/" + segment;

			return new Url(Scheme, Authority, newPath, Query, Fragment);
		}

		public static Url Parse(string url)
		{
			if (url == null)
				return null;

			if (url.StartsWith("~"))
				url = ToAbsolute(url);

			return new Url(url);
		}

		public Url PrependSegment(string segment)
		{
			if (string.IsNullOrEmpty(Path) || Path == "/")
				return PrependSegment(segment, DefaultExtension);

			return PrependSegment(segment, Extension);
		}

		public Url PrependSegment(string segment, string extension)
		{
			string newPath;
			if (string.IsNullOrEmpty(Path) || Path == "/")
				newPath = "/" + segment + extension;
			else if (extension != Extension)
			{
				newPath = "/" + segment + PathWithoutExtension + extension;
			}
			else
			{
				newPath = "/" + segment + Path;
			}

			return new Url(Scheme, Authority, newPath, Query, Fragment);
		}

		/// <summary>Removes the file extension from a path.</summary>
		/// <param name="path">The server relative path.</param>
		/// <returns>The path without the file extension or the same path if no extension was found.</returns>
		public static string RemoveExtension(string path)
		{
			int index = path.LastIndexOfAny(_dotsAndSlashes);

			if (index < 0)
				return path;
			if (path[index] == '/')
				return path;

			return path.Substring(0, index);
		}

		public Url SetFragment(string fragment)
		{
			return new Url(Scheme, Authority, Path, Query, fragment.TrimStart('#'));
		}

		public Url SetQuery(string query)
		{
			return new Url(Scheme, Authority, Path, query, Fragment);
		}

		public Url SetPath(string path)
		{
			if (path.StartsWith("~"))
				path = ToAbsolute(path);
			int queryIndex = QueryIndex(path);
			return new Url(Scheme, Authority, queryIndex < 0 ? path : path.Substring(0, queryIndex), Query, Fragment);
		}

		public Url UpdateQuery(NameValueCollection queryString)
		{
			Url u = new Url(this);
			foreach (string key in queryString.AllKeys)
				u = u.SetQueryParameter(key, queryString[key]);
			return u;
		}

		public Url UpdateQuery(IDictionary<string, string> queryString)
		{
			Url u = new Url(this);
			foreach (KeyValuePair<string, string> pair in queryString)
				u = u.SetQueryParameter(pair.Key, pair.Value);
			return u;
		}

		/// <summary>Converts a virtual path to a relative path, e.g. /myapp/path/to/a/page.aspx -> ~/path/to/a/page.aspx</summary>
		/// <param name="path">The virtual path.</param>
		/// <returns>A relative path</returns>
		public static string ToRelative(string path)
		{
			if (!string.IsNullOrEmpty(path) && path.StartsWith(ApplicationPath, StringComparison.OrdinalIgnoreCase))
				return "~/" + path.Substring(ApplicationPath.Length);
			return path;
		}

		public static string ToAbsolute(string path)
		{
			if (!string.IsNullOrEmpty(path) && path[0] == '~' && path.Length > 1)
				return ApplicationPath + path.Substring(2);
			else if (path == "~")
				return ApplicationPath;
			return path;
		}

		public override string ToString()
		{
			string url;
			if (Authority != null)
				url = Scheme + "://" + Authority + Path;
			else
				url = Path;
			if (Query != null)
				url += "?" + Query;
			if (Fragment != null)
				url += "#" + Fragment;
			return url;
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

		public string GetQuery(string key)
		{
			IDictionary<string, string> queries = GetQueries();
			if (queries.ContainsKey(key))
				return queries[key];

			return null;
		}

		public string this[string queryKey]
		{
			get { return GetQuery(queryKey); }
		}

		public IDictionary<string, string> GetQueries()
		{
			var dictionary = new Dictionary<string, string>();
			if (Query == null)
				return dictionary;

			string[] queries = Query.Split(_querySplitter, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < queries.Length; i++)
			{
				string q = queries[i];
				int eqIndex = q.IndexOf("=");
				if (eqIndex >= 0)
					dictionary[q.Substring(0, eqIndex)] = q.Substring(eqIndex + 1);
			}
			return dictionary;
		}

		/// <summary>Retrieves the query part of an url, e.g. page=12&value=something.</summary>
		public static string QueryPart(string url)
		{
			url = RemoveHash(url);

			int queryIndex = QueryIndex(url);
			if (queryIndex >= 0)
				return url.Substring(queryIndex + 1);
			return string.Empty;
		}

		#region Operators

		public static implicit operator string(Url u)
		{
			if (u == null)
				return null;
			return u.ToString();
		}

		public static implicit operator Url(string url)
		{
			return Parse(url);
		}

		#endregion
	}
}