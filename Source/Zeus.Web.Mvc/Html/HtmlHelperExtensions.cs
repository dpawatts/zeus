using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using Isis.Web.UI;
using Zeus.ContentProperties;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Web.Mvc.Html
{
	public static class HtmlHelperExtensions
	{
		public static string IncludeJavascriptResource(this HtmlHelper html, Type type, string resourceName)
		{
			return string.Format(@"<script type=""text/javascript"" src=""{0}""></script>",
				WebResourceUtility.GetUrl(type, resourceName));
		}

		public static string IncludeEmbeddedJavascriptResource(this HtmlHelper html, Assembly assembly, string resourceName)
		{
			return string.Format(@"<script type=""text/javascript"" src=""{0}""></script>",
				EmbeddedWebResourceUtility.GetUrl(assembly, resourceName));
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
	}
}