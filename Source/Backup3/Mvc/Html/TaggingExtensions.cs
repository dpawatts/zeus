using System.Collections.Generic;
using System.Web.Mvc;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Services;

namespace Zeus.Templates.Mvc.Html
{
	public static class TaggingExtensions
	{
		public static IEnumerable<Tag> Tags(this HtmlHelper html, ContentItem contentItem)
		{
			return Context.Current.Resolve<ITagService>().GetTags(contentItem);
		}
	}
}