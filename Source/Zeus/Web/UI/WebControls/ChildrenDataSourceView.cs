using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Compilation;
using System.Web.UI;
using Isis.Linq;

namespace Zeus.Web.UI.WebControls
{
	public class ChildrenDataSourceView : ContentDataSourceView
	{
		public string OfType
		{
			get;
			set;
		}

		public ChildrenDataSourceView(IDataSource owner, string viewName, ContentItem parentItem)
			: base(owner, viewName, parentItem)
		{

		}

		protected override IEnumerable<ContentItem> GetItems()
		{
			if (this.ParentItem != null)
			{
				IEnumerable<ContentItem> children = this.ParentItem.GetChildren();
				if (!string.IsNullOrEmpty(this.OfType))
				{
					Type typeFilter = BuildManager.GetType(this.OfType, true);
					children = children.OfType(typeFilter).AsQueryable();
				}
				return children;
			}
			else
				return null;
		}
	}
}
