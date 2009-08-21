using System.Web;
using System.Web.Mvc;

namespace Zeus.Templates.Mvc.Html
{
	public static class FileExtensions
	{
		public static string FileUrl(this HtmlHelper helper, ContentItem contentItem, string detailName)
		{
			return string.Format("/FileData.axd?Path={0}&DetailName={1}", HttpUtility.UrlEncode(contentItem.Path), detailName);
		}

		public static string FileUrl(this HtmlHelper helper, ContentItem contentItem, string detailCollectionName, int index)
		{
			return string.Format("/FileData.axd?Path={0}&DetailCollectionName={1}&Index={2}", HttpUtility.UrlEncode(contentItem.Path), detailCollectionName, index);
		}
	}
}