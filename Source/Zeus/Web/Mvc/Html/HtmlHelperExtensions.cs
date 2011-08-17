using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Zeus.BaseLibrary.Web;
using Zeus.BaseLibrary.Web.UI;
using Zeus.ContentProperties;
using Zeus.Web.Mvc.ViewModels;
using Zeus.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Collections;
using System.Text;

namespace Zeus.Web.Mvc.Html
{
	public static class HtmlHelperExtensions
	{
		public static string RegisterJQuery(this HtmlHelper htmlHelper)
		{
			if (htmlHelper.ViewContext.HttpContext.Items["RegisterJQuery"] != null)
				return string.Empty;

			htmlHelper.ViewContext.HttpContext.Items["RegisterJQuery"] = true;
			return htmlHelper.IncludeJavascriptResource(typeof(HtmlTextBox), "Zeus.Web.Resources.jQuery.jquery.js");
		}

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

		public static string ToAbsoluteUrl(this HtmlHelper html, string url)
		{
			return VirtualPathUtility.ToAbsolute(url);
		}

		public static Url CurrentUrl(this HtmlHelper html)
		{
			return new Url(html.ViewContext.HttpContext.Request.RawUrl);
		}

		public static Url Url<TController>(this HtmlHelper html, ContentItem contentItem, Expression<Action<TController>> action)
			where TController : Controller
		{
			// Get area name and controller name.
			var controllerMapper = Context.Current.Resolve<IControllerMapper>();

			RouteValueDictionary routeValuesFromExpression = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression(action);
			if (routeValuesFromExpression.ContainsKey(ContentRoute.ActionKey))
			{
				string actionName = routeValuesFromExpression[ContentRoute.ActionKey].ToString();
				routeValuesFromExpression[ContentRoute.ActionKey] = actionName.Substring(0, 1).ToLower() + actionName.Substring(1);
			}
			routeValuesFromExpression.Add(ContentRoute.AreaKey, controllerMapper.GetAreaName(contentItem.GetType()));
			routeValuesFromExpression.Add(ContentRoute.ContentItemKey, contentItem);
			VirtualPathData virtualPath = html.RouteCollection.GetVirtualPath(html.ViewContext.RequestContext, routeValuesFromExpression);

			if (virtualPath != null)
				return virtualPath.VirtualPath;
			return null;
		}


        /// <summary>
        /// Content cut for summaries in the lists
        /// </summary>
        /// <param name="html"></param>
        /// <param name="theString"></param>
        /// <param name="theLength"></param>
        /// <returns></returns>
        public static string SafeTruncate(this HtmlHelper html, string theString, int theLength)
        {
            if (theString.Length < theLength)
            {
                return theString;
            }
            else
            {
                string newText = "";
                newText = theString.Substring(0, theLength);

                // test whether the truncate has cut into an existing HTML tag. If it has, remove a character to newText and test again. Do this until false. 
                Regex isItCutXP = new Regex(@"<[^>]*$");
                while (isItCutXP.IsMatch(newText))
                {
                    theLength--;
                    newText = theString.Substring(0, theLength);
                }

                //remove images from newText
                Regex imagesRGX = new Regex(@"<img[^>]+>", RegexOptions.None);
                newText = imagesRGX.Replace(newText, "");

                // match all opening HTML tags (avoiding <br> tags) in newText and put in an array called 'theMatches'
                Regex openTagsRGX = new Regex(@"<(?!\/)(?!br)[^>]+>", RegexOptions.IgnoreCase);
                MatchCollection theMatches = openTagsRGX.Matches(newText);


                // for each opening tag, create a close tag
                ArrayList theCloses = new ArrayList();
                Regex inTagRGX = new Regex(@"\w+");
                foreach (Match m in theMatches)
                {

                    var theTag = inTagRGX.Match(m.ToString());
                    string toAdd = "</" + theTag.ToString() + ">";
                    theCloses.Add(toAdd);
                }


                //find all currently existing close tags
                Regex closeTagsRGX = new Regex(@"<\/[^>]+>", RegexOptions.IgnoreCase);
                MatchCollection existingCloseTags = closeTagsRGX.Matches(newText);
                string returningText = "";

                //if there are any, delete matches entries in theCloses in the order in which they appear

                foreach (Match m in existingCloseTags)
                {

                    foreach (string exC in theCloses)
                    {
                        if (m.ToString() == exC)
                        {
                            theCloses.Remove(exC);
                            break;
                        }
                    }

                }
                //reverse it
                theCloses.Reverse();

                //concatentate theCloses into a string and tack it to the end of the truncated text.
                StringBuilder theCloseString = new StringBuilder();
                foreach (string m in theCloses)
                {
                    theCloseString.Append(m);
                }

                returningText = newText + "..." + theCloseString;

                return returningText;
            }
        }

	}
}