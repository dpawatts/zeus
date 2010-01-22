using System;

namespace Zeus.Web.Caching
{
	public static class CachingExtensions
	{
		private const string PageCacheEnabledKey = "PageCache_Enabled";
		private const string PageCacheDurationKey = "PageCache_Duration";

		public static bool GetPageCachingEnabled(this ContentItem contentItem)
		{
			return contentItem.IsPage && contentItem.GetDetail(PageCacheEnabledKey, false);
		}

		public static void SetPageCachingEnabled(this ContentItem contentItem, bool enabled)
		{
			contentItem.SetDetail(PageCacheEnabledKey, enabled);
		}

		public static TimeSpan GetPageCachingDuration(this ContentItem contentItem)
		{
			return contentItem.GetDetail(PageCacheDurationKey, TimeSpan.FromHours(1));
		}

		public static void SetPageCachingDuration(this ContentItem contentItem, TimeSpan duration)
		{
			contentItem.SetDetail(PageCacheDurationKey, duration);
		}
	}
}