using System;
using System.Web.UI;
using System.Collections.Generic;
using System.Collections;

namespace Zeus.Web.UI.WebControls
{
	public class CurrentItemDataSourceView : BaseContentDataSourceView
	{
		public CurrentItemDataSourceView(IDataSource owner, string viewName, ContentItem parentItem)
			: base(owner, viewName, parentItem)
		{

		}

		protected override IEnumerable GetItems()
		{
			if (this.ParentItem != null)
				return new ContentItem[] { this.ParentItem };
			else
				return null;
		}
	}
}
