using System.Web.Mvc;

namespace Zeus.Web.Mvc.Html
{
	public static class ZoneExtensions
	{
		/// <summary>
		/// Renders all items in the Zone of the given name from the item held by the <see cref="container" />.
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="container"></param>
		/// <param name="zoneName"></param>
		/// <returns></returns>
		public static ZoneHelper Zone<TItem>(this HtmlHelper html, IContentItemContainer<TItem> container, string zoneName)
			where TItem : ContentItem
		{
			return html.Zone(container, zoneName, container.CurrentItem);
		}

		/// <summary>
		/// Renders all items in the Zone of the given name from the <see cref="item" /> given.
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="container"></param>
		/// <param name="zoneName"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ZoneHelper Zone<TItem>(this HtmlHelper html, IContentItemContainer<TItem> container, string zoneName, ContentItem item)
			where TItem : ContentItem
		{
			return new ZoneHelper(container, item, "index", zoneName);
		}
	}
}