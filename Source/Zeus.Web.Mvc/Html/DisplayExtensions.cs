using System.Web.Mvc;

namespace Zeus.Web.Mvc.Html
{
	public static class DisplayExtensions
	{
		public static DisplayHelper Display<TItem>(this HtmlHelper html, IContentItemContainer<TItem> container, ContentItem item)
			where TItem : ContentItem
		{
			return new DisplayHelper(container, item);
		}
	}
}