using System.Collections.Generic;
using Zeus.Persistence.Specifications;

namespace Zeus.Collections
{
	public abstract class HierarchyBuilder
	{
		public ISpecification<ContentItem>[] Filters
		{
			get;
			set;
		}

		public abstract HierarchyNode<ContentItem> Build();

		public HierarchyNode<ContentItem> Build(ISpecification<ContentItem>[] filters)
		{
			Filters = filters;
			return Build();
		}

		protected virtual IList<ContentItem> GetChildren(ContentItem currentItem)
		{
			return Filters == null
				? currentItem.GetChildren()
				: currentItem.GetChildren(Filters);
		}
	}
}
