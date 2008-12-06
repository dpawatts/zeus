using System;
using System.Web.UI;
using System.Collections.Generic;

namespace Zeus.Web.UI.WebControls
{
	public class CurrentItemDataSourceView : ContentDataSourceView
	{
		public CurrentItemDataSourceView(IDataSource owner, string viewName, ContentItem parentItem)
			: base(owner, viewName, parentItem)
		{

		}

		protected override IEnumerable<ContentItem> GetItems()
		{
			if (this.ParentItem != null)
				return new ContentItem[] { this.ParentItem };
			else
				return null;
		}
	}
}
