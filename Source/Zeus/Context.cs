using System;
using Zeus.Persistence;
using Zeus.Engine;

namespace Zeus
{
	public static class Context
	{
		public static ContentEngine Current
		{
			get { return new ContentEngine(); }
		}

		/// <summary>
		/// Gets the current page. This is retrieved by the page querystring.
		/// </summary>
		public static ContentItem CurrentPage
		{
			get { return Current.UrlParser.CurrentPage; }
		}

		/// <summary>
		/// Gets Zeus persistence manager used for database persistence of content.
		/// </summary>
		public static Persistence.IPersister Persister
		{
			get { return Current.Persister; }
		}
	}
}
