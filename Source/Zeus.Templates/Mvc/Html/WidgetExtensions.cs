using System.Web.Mvc;
using Zeus.Web.Mvc;
using Zeus.Web.Mvc.Html;

namespace Zeus.Templates.Mvc.Html
{
	public static class WidgetExtensions
	{
		/// <summary>
		/// Renders all headers of all widgets in the <see cref="container" />.
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="html"></param>
		/// <param name="container"></param>
		/// <returns></returns>
		public static WidgetHelper WidgetHeaders<TItem>(this HtmlHelper html, IContentItemContainer<TItem> container, params string[] zoneNames)
			where TItem : ContentItem
		{
			return new WidgetHelper(html, container, "header", zoneNames);
		}
	}
}