using System;

namespace Zeus.Web
{
	/// <summary>
	/// Exposes information about url's that can't be parsed into a page.
	/// </summary>
	public class PageNotFoundEventArgs : ItemEventArgs
	{
		public PageNotFoundEventArgs(string url)
			: base(null)
		{
			this.Url = url;
		}

		/// <summary>The url that didn't match any page.</summary>
		public string Url
		{
			get;
			set;
		}
	}
}
