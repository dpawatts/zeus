using System;
using Zeus.Linq.Filters;
using System.Collections.Generic;

namespace Zeus.Collections
{
	public abstract class HierarchyBuilder
	{
		public ItemFilter[] Filters
		{
			get;
			set;
		}

		public abstract HierarchyNode<ContentItem> Build();

		public HierarchyNode<ContentItem> Build(ItemFilter[] filters)
		{
			this.Filters = filters;
			return Build();
		}

		protected virtual IList<ContentItem> GetChildren(ContentItem currentItem)
		{
			return this.Filters == null
				? currentItem.GetChildren()
				: currentItem.GetChildren(this.Filters);
		}
	}
}
