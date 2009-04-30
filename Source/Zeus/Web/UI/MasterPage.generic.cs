namespace Zeus.Web.UI
{
	/// <summary>MasterPage base class providing easy access to current page item.</summary>
	/// <typeparam name="T">The type of content item for this masterpage</typeparam>
	public abstract class MasterPage<TPage> : System.Web.UI.MasterPage, IContentItemContainer
	where TPage : ContentItem
	{
		public virtual TPage CurrentPage
		{
			get { return (TPage) Zeus.Context.CurrentPage; }
		}

		public virtual TPage CurrentItem
		{
			get { return CurrentPage; }
		}

		#region IItemContainer Members

		ContentItem IContentItemContainer.CurrentItem
		{
			get { return CurrentPage; }
		}

		#endregion
	}
}
