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
			Url = url;
		}

		/// <summary>The url that didn't match any page.</summary>
		public string Url
		{
			get; set;
		}

		/// <summary>The template data to associate with the not found url.</summary>
		public PathData AffectedPath
		{
			get; set;
		}
	}
}
