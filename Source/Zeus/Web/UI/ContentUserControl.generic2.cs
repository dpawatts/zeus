namespace Zeus.Web.UI
{
	/// <summary>A user control that can be dynamically created, bound to non-page items and added to a page.</summary>
	/// <typeparam name="TPage">The type of page item this user control will have to deal with.</typeparam>
	/// <typeparam name="TItem">The type of non-page (data) item this user control will be bound to.</typeparam>
	public abstract class ContentUserControl<TPage, TItem> : ContentUserControl<TPage>, IContentItemContainer, IContentTemplate
		where TPage : ContentItem
		where TItem : ContentItem
	{
		private TItem currentItem = null;

		/// <summary>Gets the current data item of the dynamically added part.</summary>
		public new TItem CurrentItem
		{
			get { return currentItem ?? (currentItem = ItemUtility.CurrentContentItem as TItem); }
			set { currentItem = value; }
		}

		#region IItemContainer & IContentTemplate

		ContentItem IContentTemplate.CurrentItem
		{
			get { return CurrentItem; }
			set { CurrentItem = ItemUtility.EnsureType<TItem>(value); }
		}

		ContentItem IContentItemContainer.CurrentItem
		{
			get { return CurrentItem; }
		}

		#endregion
	}
}