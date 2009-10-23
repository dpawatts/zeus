using Zeus.Persistence;

namespace Zeus.Web
{
	public class WidgetContentItem : ContentItem
	{
		/// <summary>Gets or sets zone name which is associated with data items and their placement on a page.</summary>
		// TODO: Remove this and put it in WidgetContentItem
		[Copy]
		public virtual string ZoneName { get; set; }
	}
}