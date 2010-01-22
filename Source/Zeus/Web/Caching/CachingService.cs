using System;
using System.Web.Caching;

namespace Zeus.Web.Caching
{
	public class CachingService : ICachingService
	{
		private readonly IWebContext _webContext;

		public CachingService(IWebContext webContext)
		{
			_webContext = webContext;
		}

		public bool IsPageCached(ContentItem contentItem)
		{
			return _webContext.HttpContext.Cache[GetCacheKey(contentItem)] != null;
		}

		public void InsertCachedPage(ContentItem contentItem, string html)
		{
			_webContext.HttpContext.Cache.Insert(GetCacheKey(contentItem),
				html, null, DateTime.Now.Add(contentItem.GetPageCachingDuration()),
				Cache.NoSlidingExpiration);
		}

		public string GetCachedPage(ContentItem contentItem)
		{
			object cachedPage = _webContext.HttpContext.Cache[GetCacheKey(contentItem)];
			if (cachedPage == null)
				throw new InvalidOperationException("Page is not cached.");

			return (string) cachedPage;
		}

		public void DeleteCachedPage(ContentItem contentItem)
		{
			_webContext.HttpContext.Cache.Remove(GetCacheKey(contentItem));
		}

		private static string GetCacheKey(ContentItem contentItem)
		{
			return "ZeusPageCache_" + contentItem.ID;
		}
	}
}