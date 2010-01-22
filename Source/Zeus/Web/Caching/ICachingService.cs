namespace Zeus.Web.Caching
{
	public interface ICachingService
	{
		bool IsPageCached(ContentItem contentItem);
		void InsertCachedPage(ContentItem contentItem, string html);
		string GetCachedPage(ContentItem contentItem);
		void DeleteCachedPage(ContentItem contentItem);
	}
}