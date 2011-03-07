using System;
using System.Web;

namespace Zeus.Web
{
	public class Url
	{
		public const string Amp = "&";
		private static readonly char[] _dotsAndSlashes = new[] { '.', '/' };
		private static readonly string[] _querySplitter = new[] { "&amp;", Amp };

		public string Scheme
		{
			get;
			private set;
		}

		public string Authority
		{
			get;
			private set;
		}

		public string Path
		{
			get;
			private set;
		}

		public string Query
		{
			get;
			private set;
		}

		public string Fragment
		{
			get;
			private set;
		}

		public string Extension
		{
			get
			{
				int index = this.Path.LastIndexOfAny(_dotsAndSlashes);

				if (index < 0)
					return null;
				if (this.Path[index] == '/')
					return null;

				return this.Path.Substring(index);
			}
		}

		public Url(Url other)
		{
			this.Scheme = other.Scheme;
			this.Authority = other.Authority;
			this.Path = other.Path;
			this.Query = other.Query;
			this.Fragment = other.Fragment;
		}

		public Url(string scheme, string authority, string path, string query, string fragment)
		{
			this.Scheme = scheme;
			this.Authority = authority;
			this.Path = path;
			this.Query = query;
			this.Fragment = fragment;
		}

		public Url(string url)
		{
			if (url != null)
			{
				int queryIndex = QueryIndex(url);
				int hashIndex = url.IndexOf('#', queryIndex > 0 ? queryIndex : 0);
				int authorityIndex = url.IndexOf("://");

				if (hashIndex >= 0)
					this.Fragment = url.Substring(hashIndex + 1);
				else
					this.Fragment = null;

				if (hashIndex >= 0 && queryIndex >= 0)
					this.Query = url.Substring(queryIndex + 1, hashIndex - queryIndex - 1);
				else if (queryIndex >= 0)
					this.Query = url.Substring(queryIndex + 1);
				else
					this.Query = null;

				if (authorityIndex >= 0)
				{
					this.Scheme = url.Substring(0, authorityIndex);
					int slashIndex = url.IndexOf('/', authorityIndex + 3);
					if (slashIndex > 0)
					{
						this.Authority = url.Substring(authorityIndex + 3, slashIndex - authorityIndex - 3);
						if (queryIndex >= slashIndex)
							this.Path = url.Substring(slashIndex, queryIndex - slashIndex);
						else if (hashIndex >= 0)
							this.Path = url.Substring(slashIndex, hashIndex - slashIndex);
						else
							this.Path = url.Substring(slashIndex);
					}
					else
					{
						// is this case tolerated?
						this.Authority = url.Substring(authorityIndex + 3);
						this.Path = "/";
					}
				}
				else
				{
					this.Scheme = null;
					this.Authority = null;
					if (queryIndex >= 0)
						this.Path = url.Substring(0, queryIndex);
					else if (hashIndex >= 0)
						this.Path = url.Substring(0, hashIndex);
					else if (url.Length > 0)
						this.Path = url;
					else
						this.Path = "/";
				}
			}
			else
			{
				this.Scheme = null;
				this.Authority = null;
				this.Path = "/";
				this.Query = null;
				this.Fragment = null;
			}
		}

		public static string ApplicationPath
		{
			get { return System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath; }
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
			if (string.IsNullOrEmpty(this.Query))
				clone.Query = keyValue;
			else if (!string.IsNullOrEmpty(keyValue))
				clone.Query += Amp + keyValue;
			return clone;
		}

		public Url SetQueryParameter(string key, int value)
		{
			return SetQueryParameter(key, value.ToString());
		}

		public Url SetQueryParameter(string key, string value)
		{
			if (this.Query == null)
				return AppendQuery(key, value);

			var clone = new Url(this);
			string[] queries = this.Query.Split(_querySplitter, StringSplitOptions.RemoveEmptyEntries);
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
						clone.Query = string.Join(Amp, queries, 0, i) + Amp + string.Join(Amp, queries, i + 1, queries.Length - i - 1);
					return clone;
				}
			}
			return AppendQuery(key, value);
		}

		public Url SetQueryParameter(string keyValue)
		{
			if (this.Query == null)
				return AppendQuery(keyValue);

			int eqIndex = keyValue.IndexOf('=');
			if (eqIndex >= 0)
				return SetQueryParameter(keyValue.Substring(0, eqIndex), keyValue.Substring(eqIndex + 1));
			else
				return SetQueryParameter(keyValue, string.Empty);
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
			if (string.IsNullOrEmpty(this.Path) || this.Path == "/")
				newPath = "/" + segment + extension;
			else if (!string.IsNullOrEmpty(extension))
			{
				int extensionIndex = this.Path.LastIndexOf(extension);
				if (extensionIndex >= 0)
					newPath = this.Path.Insert(extensionIndex, "/" + segment);
				else if (this.Path.EndsWith("/"))
					newPath = this.Path + segment + extension;
				else
					newPath = this.Path + "/" + segment + extension;
			}
			else if (this.Path.EndsWith("/"))
				newPath = this.Path + segment;
			else
				newPath = this.Path + "/" + segment;

			return new Url(this.Scheme, this.Authority, newPath, this.Query, this.Fragment);
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
			string url;
			if (this.Authority != null)
				url = this.Scheme + "://" + this.Authority + this.Path;
			else
				url = this.Path;
			if (this.Query != null)
				url += "?" + this.Query;
			if (this.Fragment != null)
				url += "#" + this.Fragment;
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
	}
}
