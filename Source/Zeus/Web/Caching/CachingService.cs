using System;
using System.Web.Caching;
using Ninject;
using Zeus.Persistence;

namespace Zeus.Web.Caching
{
	public class CachingService : ICachingService, IStartable
	{
		private readonly IWebContext _webContext;
		private readonly IPersister _persister;

		public CachingService(IWebContext webContext, IPersister persister)
		{
			_webContext = webContext;
			_persister = persister;
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

		#region IStartable methods

		public void Start()
		{
			_persister.ItemSaving += OnPersisterItemSaving;
		}

		private void OnPersisterItemSaving(object sender, CancelItemEventArgs e)
		{
			if (IsPageCached(e.AffectedItem))
				DeleteCachedPage(e.AffectedItem);
		}

		public void Stop()
		{
			_persister.ItemSaving -= OnPersisterItemSaving;
		}

		#endregion
	}
}