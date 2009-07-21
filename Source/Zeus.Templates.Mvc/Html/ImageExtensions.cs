using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Zeus.FileSystem.Images;

namespace Zeus.Templates.Mvc.Html
{
	public static class ImageExtensions
	{
		public static string FileUrl(this HtmlHelper helper, ContentItem contentItem, string detailName)
		{
			return string.Format("/FileData.axd?Path={0}&DetailName={1}", HttpUtility.UrlEncode(contentItem.Path), detailName);
		}

		public static string FileUrl(this HtmlHelper helper, ContentItem contentItem, string detailCollectionName, int index)
		{
			return string.Format("/FileData.axd?Path={0}&DetailCollectionName={1}&Index={2}", HttpUtility.UrlEncode(contentItem.Path), detailCollectionName, index);
		}

		public static string ImageUrl(this HtmlHelper helper, ContentItem contentItem, string detailName, int width, int height, bool fill)
		{
			return contentItem.GetImageUrl(detailName, width, height, fill);
		}

		public static string ImageUrl<TItem>(this HtmlHelper helper, TItem contentItem, Expression<Func<TItem, object>> expression, int width, int height, bool fill)
			where TItem : ContentItem
		{
			MemberExpression memberExpression = (MemberExpression) expression.Body;
			return helper.ImageUrl(contentItem, memberExpression.Member.Name, width, height, fill);
		}

		public static string ImageUrl(this HtmlHelper helper, ContentItem contentItem, string detailName)
		{
			return contentItem.GetImageUrl(detailName);
		}

		public static string ImageUrl<TItem>(this HtmlHelper helper, TItem contentItem, Expression<Func<TItem, object>> expression)
			where TItem : ContentItem
		{
			MemberExpression memberExpression = (MemberExpression)expression.Body;
			return helper.ImageUrl(contentItem, memberExpression.Member.Name);
		}

		public static string ImageUrl(this HtmlHelper helper, ContentItem contentItem, string detailCollectionName, int index, int width, int height, bool fill)
		{
			return contentItem.GetImageUrl(detailCollectionName, index, width, height, fill);
		}

		public static string ImageUrl<TItem>(this HtmlHelper helper, TItem contentItem, Expression<Func<TItem, object>> expression, int index, int width, int height, bool fill)
			where TItem : ContentItem
		{
			MemberExpression memberExpression = (MemberExpression)expression.Body;
			return helper.ImageUrl(contentItem, memberExpression.Member.Name, index, width, height, fill);
		}
	}
}