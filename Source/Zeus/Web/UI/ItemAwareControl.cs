﻿using System.Web.UI;

namespace Zeus.Web.UI
{
	/// <summary>
	/// Base class for controls that looks upwards in the control hierarchy 
	/// to find the current item.
	/// </summary>
	public abstract class ItemAwareControl : Control, IContentItemContainer
	{
		private ContentItem currentItem;

		/// <summary>Gets or sets a relative path to the item whose children should be listed by this datasource.</summary>
		/// <example>
		/// ctrl.Path = ".."; // Display other items on the same level.
		/// ctrl.Path = "lorem"; // Display children of the item with the name 'lorem' below the current item.
		/// ctrl.Path = "/ipsum"; // Display children of the item with the name 'ipsum' directly below the start page.
		/// ctrl.Path = "//dolor"; // Display children of the item with the name 'dolor' directly below the root item (this is the same as the start page unless multiple hosts are configured).
		/// ctrl.Path = "/"; // Display children of the start page.
		/// </example>
		public virtual string Path
		{
			get { return (string) ViewState["Path"] ?? ""; }
			set
			{
				ViewState["Path"] = value;
				currentItem = null;
			}
		}

		protected virtual ContentItem CurrentPage
		{
			get { return (ContentItem) ((Page is IContentTemplate) ? (Page as IContentTemplate).CurrentItem : Zeus.Find.CurrentPage); }
		}

		public virtual ContentItem CurrentItem
		{
			get
			{
				if (currentItem == null)
				{
					currentItem = this.FindCurrentItem();
					if (Path.Length > 0)
						currentItem = ItemUtility.WalkPath(currentItem, Path);
				}
				return currentItem;
			}
			set
			{
				currentItem = value;
			}
		}
	}
}