using System;
using Zeus.Configuration;
using Zeus.Persistence;
using System.Web;

namespace Zeus.Web
{
	public class UrlParser : IUrlParser
	{
		private IPersister _persister;
		private Host _host;
		private IWebContext _webContext;

		public event EventHandler<PageNotFoundEventArgs> PageNotFound;

		public UrlParser(IPersister persister, Host host, IWebContext webContext, IItemNotifier notifier)
		{
			_persister = persister;
			_host = host;
			_webContext = webContext;

			notifier.ItemCreated += OnItemCreated;

			this.DefaultContentPage = "/default.aspx";
		}

		public string DefaultContentPage
		{
			get;
			set;
		}

		public ContentItem StartPage
		{
			get { return _persister.Load(_host.StartPageID); }
		}

		public ContentItem CurrentPage
		{
			get
			{
				return _webContext.CurrentPage ?? (_webContext.CurrentPage = ParsePage(_webContext.LocalUrl.ToString()));
			}
		}

		public string BuildUrl(ContentItem item)
		{
			ContentItem current = item;
			string url = string.Empty;

			// Walk the item's parent items to compute it's url
			do
			{
				if (IsStartPage(current))
					return ToAbsolute(url, item);
				if (current.IsPage)
					url = "/" + current.Name + url;
				current = current.Parent;
			} while (current != null);

			// If we didn't find the start page, it means the specified
			// item is not part of the current site.
			return item.RewrittenUrl;
		}

		protected virtual bool IsStartPage(ContentItem item)
		{
			return item.ID == _host.StartPageID;
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

			if (item.IsPage)
				return url;
			else
				return url + "?item=" + item.ID;
		}

		/// <summary>Checks if an item is startpage or root page</summary>
		/// <param name="item">The item to compare</param>
		/// <returns>True if the item is a startpage or a rootpage</returns>
		public virtual bool IsRootOrStartPage(ContentItem item)
		{
			return item.ID == _host.RootItemID || IsStartPage(item);
		}

		/// <summary>Invoked when an item is created or loaded from persistence medium.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void OnItemCreated(object sender, ItemEventArgs e)
		{
			((IUrlParserDependency) e.AffectedItem).SetUrlParser(this);
		}

		public virtual ContentItem ParsePage(string url)
		{
			if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url");
			return TryLoadingFromQueryString(url) ?? Parse(StartPage, url);
		}

		protected virtual ContentItem TryLoadingFromQueryString(string url)
		{
			if (HttpContext.Current.Request.QueryString["page"] != null)
				return _persister.Get(Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]));
			return null;
		}

		protected virtual ContentItem Parse(ContentItem current, string url)
		{
			if (current == null) throw new ArgumentNullException("current");

			url = CleanUrl(url);

			if (url.Length == 0)
				return current;
			else
				return current.GetChild(url) ?? NotFoundPage(url);
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
			string defaultDocument = CleanUrl(DefaultContentPage);
			if (url.Equals(defaultDocument, StringComparison.InvariantCultureIgnoreCase))
				return this.StartPage;

			PageNotFoundEventArgs args = new PageNotFoundEventArgs(url);
			if (PageNotFound != null)
				PageNotFound(this, args);
			return args.AffectedItem;
		}
	}
}
