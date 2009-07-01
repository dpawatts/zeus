using System;
using System.IO;
using Isis.Web;
using Zeus.Configuration;
using Zeus.Globalization;
using Zeus.Globalization.ContentTypes;
using Zeus.Persistence;

namespace Zeus.Web
{
	public class UrlParser : IUrlParser
	{
		#region Fields

		private readonly IPersister _persister;
		private readonly IHost _host;
		private readonly IWebContext _webContext;
		private readonly bool _ignoreExistingFiles;
		private readonly ILanguageManager _languageManager;
		private readonly bool _useBrowserLanguagePreferences;

		#endregion

		#region Constructors

		public UrlParser(IPersister persister, IHost host, IWebContext webContext, IItemNotifier notifier, HostSection config, ILanguageManager languageManager)
			: this(persister, host, webContext, notifier, config, languageManager, null)
		{
			
		}

		public UrlParser(IPersister persister, IHost host, IWebContext webContext, IItemNotifier notifier, HostSection config, ILanguageManager languageManager, GlobalizationSection globalizationConfig)
		{
			_persister = persister;
			_host = host;
			_webContext = webContext;

			_ignoreExistingFiles = config.Web.IgnoreExistingFiles;

			notifier.ItemCreated += OnItemCreated;

			_languageManager = languageManager;

			DefaultDocument = "default";

			_useBrowserLanguagePreferences = (globalizationConfig != null) ? globalizationConfig.UseBrowserLanguagePreferences : false;
		}

		#endregion

		#region Events

		public event EventHandler<PageNotFoundEventArgs> PageNotFound;

		#endregion

		#region Properties

		protected IHost Host
		{
			get { return _host; }
		}

		protected IPersister Persister
		{
			get { return _persister; }
		}

		/// <summary>Gets or sets the default content document name. This is usually "/default.aspx".</summary>
		public string DefaultDocument { get; set; }

		/// <summary>Gets the current start page.</summary>
		public virtual ContentItem StartPage
		{
			get { return _persister.Repository.Load(_host.CurrentSite.StartPageID); }
		}

		public ContentItem CurrentPage
		{
			get { return _webContext.CurrentPage ?? (_webContext.CurrentPage = (ResolvePath(_webContext.Url).CurrentItem)); }
		}

		#endregion

		#region Methods

		public string BuildUrl(ContentItem item)
		{
			return BuildUrl(item, item.Language);
		}

		public virtual string BuildUrl(ContentItem item, string languageCode)
		{
			ContentItem startPage;
			return BuildUrlInternal(item, languageCode, out startPage);
		}

		protected string BuildUrlInternal(ContentItem item, string languageCode, out ContentItem startPage)
		{
			startPage = null;
			ContentItem current = item;
			Url url = new Url("/");

			// Walk the item's parent items to compute it's url
			do
			{
				if (IsStartPage(current))
				{
					startPage = current;

					if (item.VersionOf != null)
						url = url.AppendQuery("page", item.ID);

					// Prepend language identifier, if this is not the default language.
					if (!LanguageSelection.IsHostLanguageMatch(ContentLanguage.PreferredCulture.Name))
					{
						if (LanguageSelection.GetHostFromLanguage(ContentLanguage.PreferredCulture.Name) != _webContext.LocalUrl.Authority)
							url = url.SetAuthority(LanguageSelection.GetHostFromLanguage(ContentLanguage.PreferredCulture.Name));
						else if (!string.IsNullOrEmpty(languageCode))
							url = url.PrependSegment(languageCode);
					}

					// we've reached the start page so we're done here
					return url;
					//return Url.ToAbsolute("~" + url.PathAndQuery);
				}

				url = url.PrependSegment(current.Name, current.Extension);

				current = current.GetParent();
			} while (current != null);

			// If we didn't find the start page, it means the specified
			// item is not part of the current site.
			return item.FindPath(PathData.DefaultAction).RewrittenUrl;
		}

		protected virtual bool IsStartPage(ContentItem item)
		{
			return IsStartPage(item, _host.CurrentSite);
		}

		protected static bool IsStartPage(ContentItem item, Site site)
		{
			return item.ID == site.StartPageID || (item.TranslationOf != null && item.TranslationOf.ID == site.StartPageID);
		}

		/// <summary>Handles virtual directories and non-page items.</summary>
		/// <param name="url">The relative url.</param>
		/// <param name="item">The item whose url is supplied.</param>
		/// <returns>The absolute url to the item.</returns>
		protected virtual string ToAbsolute(string url, ContentItem item)
		{
			if (string.IsNullOrEmpty(url) || url == "/")
				url = _webContext.ToAbsolute("~/");
			else
				url = _webContext.ToAbsolute("~" + url + item.Extension);

			return url;
		}

		/// <summary>Checks if an item is startpage or root page</summary>
		/// <param name="item">The item to compare</param>
		/// <returns>True if the item is a startpage or a rootpage</returns>
		public virtual bool IsRootOrStartPage(ContentItem item)
		{
			return item.ID == _host.CurrentSite.RootItemID || IsStartPage(item);
		}

		/// <summary>Invoked when an item is created or loaded from persistence medium.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void OnItemCreated(object sender, ItemEventArgs e)
		{
			((IUrlParserDependency) e.AffectedItem).SetUrlParser(this);
		}

		private int? FindQueryStringReference(string url, params string[] parameters)
		{
			string queryString = Url.QueryPart(url);
			if (!string.IsNullOrEmpty(queryString))
			{
				string[] queries = queryString.Split('&');

				foreach (string parameter in parameters)
				{
					int parameterLength = parameter.Length + 1;
					foreach (string query in queries)
					{
						if (query.StartsWith(parameter + "=", StringComparison.InvariantCultureIgnoreCase))
						{
							int id;
							if (int.TryParse(query.Substring(parameterLength), out id))
							{
								return id;
							}
						}
					}
				}
			}
			return null;
		}

		protected virtual ContentItem TryLoadingFromQueryString(string url, params string[] parameters)
		{
			int? itemID = FindQueryStringReference(url, parameters);
			if (itemID.HasValue)
				return _persister.Get < ContentItem>(itemID.Value);
			return null;
		}

		protected virtual ContentItem Parse(ContentItem current, string url)
		{
			if (current == null) throw new ArgumentNullException("current");

			url = CleanUrl(url);

			if (url.Length == 0)
				return current;

			// Check if start of URL contains a language identifier.
			foreach (Language language in Context.Current.LanguageManager.GetAvailableLanguages())
				if (url.StartsWith(language.Name, StringComparison.InvariantCultureIgnoreCase))
					throw new NotImplementedException();
			
			return current.GetChild(url) ?? NotFoundPage(url);
		}

		/// <summary>May be overridden to provide custom start page depending on authority.</summary>
		/// <param name="url">The host name and path information.</param>
		/// <returns>The configured start page.</returns>
		protected virtual ContentItem GetStartPage(Url url)
		{
			return StartPage;
		}

		/// <summary>Finds an item by traversing names from the start page.</summary>
		/// <param name="url">The url that should be traversed.</param>
		/// <returns>The content item matching the supplied url.</returns>
		public virtual ContentItem Parse(string url)
		{
			if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url");

			ContentItem startingPoint = GetStartPage(url);
			return TryLoadingFromQueryString(url, PathData.ItemQueryKey, PathData.PageQueryKey) ?? Parse(startingPoint, url);
		}

		private string CleanUrl(string url)
		{
			url = Url.PathPart(url);
			url = _webContext.ToAppRelative(url);
			url = url.TrimStart('~', '/');
			if (url.EndsWith(".aspx", StringComparison.InvariantCultureIgnoreCase))
				url = url.Substring(0, url.Length - ".aspx".Length);
			return url;
		}

		protected virtual ContentItem NotFoundPage(string url)
		{
			if (url.Equals(DefaultDocument, StringComparison.InvariantCultureIgnoreCase))
				return StartPage;

			PageNotFoundEventArgs args = new PageNotFoundEventArgs(url);
			if (PageNotFound != null)
				PageNotFound(this, args);
			return args.AffectedItem;
		}

		public PathData ResolvePath(string url)
		{
			Url requestedUrl = url;
			ContentItem item = TryLoadingFromQueryString(requestedUrl, PathData.PageQueryKey);
			if (item != null)
				return item.FindPath(requestedUrl["action"] ?? PathData.DefaultAction)
					.SetArguments(requestedUrl["arguments"])
					.UpdateParameters(requestedUrl.GetQueries());

			ContentItem startPage = GetStartPage(requestedUrl);
			string languageCode = GetLanguage(ref requestedUrl);
			string path = Url.ToRelative(requestedUrl.PathWithoutExtension).TrimStart('~');
			PathData data = startPage.FindPath(path, languageCode).UpdateParameters(requestedUrl.GetQueries());

			if (data.IsEmpty())
			{
				if (path.EndsWith(DefaultDocument, StringComparison.OrdinalIgnoreCase))
				{
					// Try to find path without default document.
					data = StartPage
						.FindPath(path.Substring(0, path.Length - DefaultDocument.Length))
						.UpdateParameters(requestedUrl.GetQueries());
				}

				if (data.IsEmpty())
				{
					// Allow user code to set path through event
					if (PageNotFound != null)
					{
						PageNotFoundEventArgs args = new PageNotFoundEventArgs(requestedUrl);
						args.AffectedPath = data;
						PageNotFound(this, args);

						if (args.AffectedItem != null)
							data = args.AffectedItem.FindPath(PathData.DefaultAction);
						else
							data = args.AffectedPath;
					}
				}
			}

			data.IsRewritable = IsRewritable(_webContext.PhysicalPath);
			return data;
		}

		protected virtual string GetLanguage(ref Url url)
		{
			// Check if start of URL contains a language identifier.
			string priorityLanguage = null;
			foreach (Language language in Context.Current.LanguageManager.GetAvailableLanguages())
				if (url.Path.Equals("/" + language.Name, StringComparison.InvariantCultureIgnoreCase) || url.Path.StartsWith("/" + language.Name + "/", StringComparison.InvariantCultureIgnoreCase))
				{
					url = url.RemoveSegment(0);
					priorityLanguage = language.Name;
				}

			ContentLanguage.Instance.SetCulture(priorityLanguage);
			return ContentLanguage.PreferredCulture.Name;
		}

		private bool IsRewritable(string path)
		{
			return _ignoreExistingFiles || (!File.Exists(path) && !Directory.Exists(path));
		}

		#endregion
	}
}
