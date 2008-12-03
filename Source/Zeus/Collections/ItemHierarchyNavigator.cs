using System;
using System.Collections.Generic;

namespace Zeus.Collections
{
	/// <summary>
	/// Navigates a graph of content item nodes.
	/// </summary>
	public class ItemHierarchyNavigator : IHierarchyNavigator<ContentItem>
	{
		private readonly HierarchyNode<ContentItem> _currentNode = null;

		public ItemHierarchyNavigator(HierarchyNode<ContentItem> currentNode)
		{
			_currentNode = currentNode;
		}

		public ItemHierarchyNavigator(HierarchyBuilder builder)
		{
			_currentNode = builder.Build();
		}

		public HierarchyNode<ContentItem> CurrentNode
		{
			get { return _currentNode; }
		}

		public ItemHierarchyNavigator GetRootHierarchy()
		{
			return new ItemHierarchyNavigator(GetRootNode());
		}

		public HierarchyNode<ContentItem> GetRootNode()
		{
			HierarchyNode<ContentItem> last = _currentNode;
			while (last.Parent != null)
				last = last.Parent;
			return last;
		}

		public IEnumerable<ContentItem> EnumerateAllItems()
		{
			HierarchyNode<ContentItem> rootNode = GetRootNode();
			return EnumerateItemsRecursive(rootNode);
		}

		public IEnumerable<ContentItem> EnumerateChildItems()
		{
			return EnumerateItemsRecursive(_currentNode);
		}

		protected virtual IEnumerable<ContentItem> EnumerateItemsRecursive(HierarchyNode<ContentItem> node)
		{
			yield return node.Current;
			foreach (HierarchyNode<ContentItem> childNode in node.Children)
				foreach (ContentItem childItem in EnumerateItemsRecursive(childNode))
					yield return childItem;
		}

		public IHierarchyNavigator<ContentItem> Parent
		{
			get
			{
				if (_currentNode.Parent != null)
					return new ItemHierarchyNavigator(_currentNode.Parent);
				else
					return null;
			}
		}

		public IEnumerable<IHierarchyNavigator<ContentItem>> Children
		{
			get
			{
				foreach (HierarchyNode<ContentItem> childNode in _currentNode.Children)
					yield return new ItemHierarchyNavigator(childNode);
			}
		}

		public ContentItem Current
		{
			get { return _currentNode.Current; }
		}

		public bool HasChildren
		{
			get { return _currentNode.Children.Count > 0; }
		}
	}
}
