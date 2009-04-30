using Zeus.Engine;

namespace Zeus.Web.UI
{
	/// <summary>
	/// A user control base used to for quick access to content data.
	/// </summary>
	/// <typeparam name="TPage">The type of page item this user control will have to deal with.</typeparam>
	public abstract class ContentUserControl<TPage> : System.Web.UI.UserControl, IContentItemContainer
		where TPage : ContentItem
	{
		/// <summary>Gets the current CMS Engine.</summary>
		public ContentEngine Engine
		{
			get { return Zeus.Context.Current; }
		}

		/// <summary>Gets the current page item.</summary>
		public virtual TPage CurrentPage
		{
			get
			{
				if (CurrentItem == null)
				{
					IContentItemContainer page = Page as IContentItemContainer;
					ContentItem item = (page != null) ? page.CurrentItem : Zeus.Context.CurrentPage;
					CurrentItem = ItemUtility.EnsureType<TPage>(item);
				}
				return CurrentItem;
			}
		}

		/// <summary>This is most likely the same as CurrentPage unles you're in a user control dynamically added as a part.</summary>
		public virtual TPage CurrentItem { get; private set; }

		ContentItem IContentItemContainer.CurrentItem
		{
			get { return CurrentPage; }
		}
	}
}