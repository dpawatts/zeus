using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using Zeus.BaseLibrary.Web;
using Zeus.BaseLibrary.Web.UI;
using Zeus.ContentProperties;
using Zeus.Web.Hosting;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Web.Mvc.Html
{
	public static class HtmlHelperExtensions
	{
		public static string IncludeJavascriptResource(this HtmlHelper html, Type type, string resourceName)
		{
			return html.Javascript(WebResourceUtility.GetUrl(type, resourceName));
		}

		public static string IncludeEmbeddedJavascriptResource(this HtmlHelper html, Assembly assembly, string relativePath)
		{
			return html.Javascript(Utility.GetClientResourceUrl(assembly, relativePath));
		}

		public static string Javascript(this HtmlHelper html, string url)
		{
			return string.Format(@"<script type=""text/javascript"" src=""{0}""></script>", url);
		}

		public static string Css(this HtmlHelper html, string url)
		{
			return string.Format(@"<link rel=""stylesheet"" type=""text/css"" href=""{0}"" />", url);
		}

		public static string DisplayProperty<TItem>(this HtmlHelper helper, TItem contentItem, Expression<Func<TItem, object>> expression)
			where TItem : ContentItem
		{
			MemberExpression memberExpression = (MemberExpression) expression.Body;

			if (!contentItem.Details.ContainsKey(memberExpression.Member.Name))
				return string.Empty;

			PropertyData propertyData = contentItem.Details[memberExpression.Member.Name];
			return propertyData.GetXhtmlValue();
		}

		public static string DisplayProperty<TModel, TItem>(this HtmlHelper<TModel> helper, Expression<Func<TItem, object>> expression)
			where TModel : ViewModel<TItem>
			where TItem : ContentItem
		{
			ContentItem contentItem = helper.ViewData.Model.CurrentItem;

			MemberExpression memberExpression = (MemberExpression)expression.Body;

			if (!contentItem.Details.ContainsKey(memberExpression.Member.Name))
				return string.Empty;

			PropertyData propertyData = contentItem.Details[memberExpression.Member.Name];
			return propertyData.GetXhtmlValue();
		}

		public static Url Url(this HtmlHelper html, ContentItem contentItem)
		{
			return new Url(contentItem.Url);
		}

		public static Url Url(this HtmlHelper html, string url)
		{
			return new Url(url);
		}

		public static Url CurrentUrl(this HtmlHelper html)
		{
			return new Url(html.ViewContext.HttpContext.Request.RawUrl);
		}
	}
}