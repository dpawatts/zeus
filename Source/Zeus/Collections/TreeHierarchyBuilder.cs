namespace Zeus.Collections
{
	public class TreeHierarchyBuilder : HierarchyBuilder
	{
		private readonly ContentItem _rootItem;
		private readonly int _maxDepth;

		public TreeHierarchyBuilder(ContentItem rootItem)
			: this(rootItem, int.MaxValue)
		{

		}

		public TreeHierarchyBuilder(ContentItem rootItem, int maxDepth)
		{
			_rootItem = rootItem;
			_maxDepth = maxDepth;
		}

		public override HierarchyNode<ContentItem> Build()
		{
			return BuildTree(_rootItem, _maxDepth);
		}

		protected virtual HierarchyNode<ContentItem> BuildTree(ContentItem currentItem, int remainingDepth)
		{
			HierarchyNode<ContentItem> node = new HierarchyNode<ContentItem>(currentItem);
			if (remainingDepth > 1)
				foreach (ContentItem childItem in GetChildren(currentItem))
					node.Children.Add(BuildTree(childItem, remainingDepth - 1));
			return node;
		}
	}
}
