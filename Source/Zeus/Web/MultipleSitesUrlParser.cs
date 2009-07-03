using System;
using System.Collections.Generic;
using Isis.Web;
using Zeus.Configuration;
using Zeus.Globalization;
using Zeus.Persistence;

namespace Zeus.Web
{
	/// <summary>
	/// Parses urls in a multiple host environment.
	/// </summary>
	public class MultipleSitesUrlParser : UrlParser
	{
		#region Constructors

		public MultipleSitesUrlParser(IPersister persister, IWebContext webContext, IItemNotifier notifier, IHost host, HostSection config, ILanguageManager languageManager)
			: base(persister, host, webContext, notifier, config, languageManager)
		{
			
		}

		public MultipleSitesUrlParser(IPersister persister, IWebContext webContext, IItemNotifier notifier, IHost host, HostSection config, ILanguageManager languageManager, GlobalizationSection globalizationConfig)
			: base(persister, host, webContext, notifier, config, languageManager, globalizationConfig)
		{

		}

		#endregion

		public override ContentItem Parse(string url)
		{
			if (url.StartsWith("/") || url.StartsWith("~/"))
				return base.Parse(url);

			Site site = Host.GetSite(url);
			if (site != null)
				return TryLoadingFromQueryString(url, PathData.ItemQueryKey, PathData.PageQueryKey)
					?? Parse(Persister.Get(site.StartPageID), Url.Parse(url).PathAndQuery);

			return TryLoadingFromQueryString(url, PathData.ItemQueryKey, PathData.PageQueryKey);
		}

		public override ContentItem StartPage
		{
			get { return GetStartPage(_webContext.Url); }
		}

		protected override ContentItem GetStartPage(Url url)
		{
			if (!url.IsAbsolute)
				return StartPage;
			Site site = Host.GetSite(url) ?? Host.CurrentSite;
			return Persister.Get(site.StartPageID);
		}

		public override string BuildUrl(ContentItem item, string languageCode)
		{
			ContentItem startPage;
			Url url = BuildUrlInternal(item, languageCode, out startPage);

			if (startPage != null)
			{
				if (startPage.ID == Host.CurrentSite.StartPageID)
				{
					// the start page belongs to the current site, use relative url
					return url;
					//return Url.ToAbsolute("~" + url.PathAndQuery);
				}

				// find the start page and use it's host name
				foreach (Site site in Host.Sites)
					if (startPage.ID == site.StartPageID)
						return GetHostedUrl(item, url, site);
			}

			return url;
		}

		private static string GetHostedUrl(ContentItem item, string url, Site site)
		{
			string hostName = site.GetHostName(item.Language);
			if (string.IsNullOrEmpty(hostName))
				return item.FindPath(PathData.DefaultAction).RewrittenUrl;

			return Url.Parse(url).SetAuthority(hostName);
		}

		protected override bool IsStartPage(ContentItem item)
		{
			foreach (Site site in Host.Sites)
				if (IsStartPage(item, site))
					return true;
			return base.IsStartPage(item);
		}
	}
}