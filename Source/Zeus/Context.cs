using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Isis.Collections;
using Zeus.Persistence;
using Zeus.Engine;
using System.Web;
using Zeus.Web;

namespace Zeus
{
	public static class Context
	{
		public static ContentEngine Current
		{
			get
			{
				if (Singleton<ContentEngine>.Instance == null)
					Initialize(false);
				return Singleton<ContentEngine>.Instance;
			}
		}

		public static Admin.IAdminManager AdminManager
		{
			get { return Current.AdminManager; }
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

		public static IFinder Finder
		{
			get { return Current.Finder; }
		}

		/// <summary>
		/// Gets Zeus persistence manager used for database persistence of content.
		/// </summary>
		public static Persistence.IPersister Persister
		{
			get { return Current.Persister; }
		}

		public static Security.ISecurityManager SecurityManager
		{
			get { return Current.SecurityManager; }
		}

		public static IUrlParser UrlParser
		{
			get { return Current.UrlParser; }
		}

		/// <summary>Initializes a static instance of the Zeus context.</summary>
		/// <param name="forceRecreate">Creates a new context instance even though the context has been previously initialized.</param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static ContentEngine Initialize(bool forceRecreate)
		{
			if (Singleton<ContentEngine>.Instance == null || forceRecreate)
			{
				Debug.WriteLine("Constructing engine " + DateTime.Now);
				Singleton<ContentEngine>.Instance = CreateEngineInstance();
				Debug.WriteLine("Initializing engine " + DateTime.Now);
				Singleton<ContentEngine>.Instance.Initialize();
			}
			return Singleton<ContentEngine>.Instance;
		}

		private static ContentEngine CreateEngineInstance()
		{
			return new ContentEngine(EventBroker.Instance);
		}
	}
}
