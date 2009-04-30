using System.Web.Mvc;
using Zeus.FileSystem.Images;
using Zeus.Templates;

namespace Zeus.Templates.Mvc
{
	public static class HtmlHelperExtensions
	{
		public static string ImageUrl(this HtmlHelper helper, ContentItem contentItem, string detailName, int width, int height, bool fill)
		{
			return contentItem.GetImageUrl(detailName, width, height, fill);
		}

		public static string ImageUrl(this HtmlHelper helper, ContentItem contentItem, string detailCollectionName, int index, int width, int height, bool fill)
		{
			return contentItem.GetImageUrl(detailCollectionName, index, width, height, fill);
		}

		public static string ImageUrl(this HtmlHelper helper, ContentItem contentItem, string detailName)
		{
			return contentItem.GetImageUrl(detailName);
		}
	}
}