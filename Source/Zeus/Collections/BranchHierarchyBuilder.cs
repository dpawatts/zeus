using System;

namespace Zeus.Collections
{
	/// <summary>
	/// Builds a hierarchy of nodes between a certain item and one of it's 
	/// ancestors (or the root item). This is useful in certain situations when
	/// creating a navigation menu.
	/// </summary>
	public class BranchHierarchyBuilder : HierarchyBuilder
	{
		private readonly ContentItem _initialItem;
		private readonly ContentItem _lastAncestor;
		private readonly bool _appendAdditionalLevel = false;

		public BranchHierarchyBuilder(ContentItem initialItem, ContentItem lastAncestor, bool appendAdditionalLevel)
		{
			_initialItem = initialItem;
			_lastAncestor = lastAncestor;
			_appendAdditionalLevel = appendAdditionalLevel;
		}

		public BranchHierarchyBuilder(ContentItem initialItem, ContentItem lastAncestor)
		{
			_initialItem = initialItem;
			_lastAncestor = lastAncestor;
		}

		public override HierarchyNode<ContentItem> Build()
		{
			if (_initialItem == _lastAncestor && !_appendAdditionalLevel)
				return new HierarchyNode<ContentItem>(_initialItem);

			HierarchyNode<ContentItem> previousNode = null;
			foreach (ContentItem currentItem in Find.EnumerateParents(_initialItem, _lastAncestor, _appendAdditionalLevel))
			{
				HierarchyNode<ContentItem> currentNode = new HierarchyNode<ContentItem>(currentItem);
				if (previousNode != null)
					previousNode.Parent = currentNode;

				foreach (ContentItem childItem in currentItem.GetChildren())
				{
					if (previousNode != null && previousNode.Current.ID == childItem.ID)
					{
						currentNode.Children.Add(previousNode);
					}
					else
					{
						HierarchyNode<ContentItem> childNode = new HierarchyNode<ContentItem>(childItem);
						currentNode.Children.Add(childNode);
						childNode.Parent = currentNode;
					}
				}
				previousNode = currentNode;
			}
			return previousNode;
		}
	}
}
