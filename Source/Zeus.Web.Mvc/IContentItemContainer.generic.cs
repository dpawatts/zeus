using Zeus.Web.UI;

namespace Zeus.Web.Mvc
{
	public interface IContentItemContainer<TItem> : IContentItemContainer
		where TItem : ContentItem
	{
		new TItem CurrentItem { get; }
	}
}