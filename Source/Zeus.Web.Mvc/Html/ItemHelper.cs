using Zeus.Engine;
using Zeus.Web.Parts;
using Zeus.Web.UI;

namespace Zeus.Web.Mvc.Html
{
	public abstract class ItemHelper
	{
		private readonly IContentItemContainer _itemContainer;
		private PartsAdapter _partsAdapter;

		protected ItemHelper(IContentItemContainer itemContainer)
		{
			_itemContainer = itemContainer;
			CurrentItem = itemContainer.CurrentItem;
		}

		protected ItemHelper(IContentItemContainer itemContainer, ContentItem item)
		{
			_itemContainer = itemContainer;
			CurrentItem = item;
		}

		protected virtual ContentEngine Engine
		{
			get { return Context.Current; }
		}

		protected IContentItemContainer Container
		{
			get { return _itemContainer; }
		}

		protected ContentItem CurrentItem { get; private set; }

		/// <summary>The content adapter related to the current page item.</summary>
		protected virtual PartsAdapter PartsAdapter
		{
			get
			{
				return _partsAdapter ?? (_partsAdapter = Engine.Resolve<IContentAdapterProvider>()
																									.ResolveAdapter<PartsAdapter>(CurrentItem.FindPath(PathData.DefaultAction)));
			}
		}
	}
}