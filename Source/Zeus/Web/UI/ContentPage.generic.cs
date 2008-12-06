using System;

namespace Zeus.Web.UI
{
	public abstract class ContentPage<TContentItem> : System.Web.UI.Page, IContentTemplate, IContentItemContainer
		where TContentItem : ContentItem
	{
		public TContentItem CurrentItem
		{
			get;
			set;
		}

		ContentItem IContentTemplate.CurrentItem
		{
			get { return this.CurrentItem; }
			set { this.CurrentItem = (TContentItem) value; }
		}

		ContentItem IContentItemContainer.CurrentItem
		{
			get { return this.CurrentItem; }
		}
	}
}
