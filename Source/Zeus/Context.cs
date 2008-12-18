using System;
using Zeus.Persistence;
using Zeus.Engine;
using System.Web;
using Zeus.Web;

namespace Zeus
{
	public static class Context
	{
		private static ContentEngine _engine;

		public static ContentEngine Current
		{
			get
			{
				if (_engine == null)
					_engine = new ContentEngine();
				return _engine;
			}
		}

		public static ContentTypes.IContentTypeManager ContentTypes
		{
			get { return Current.ContentTypes; }
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

		public static IUrlParser UrlParser
		{
			get { return Current.UrlParser; }
		}
	}
}
