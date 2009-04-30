using System.Collections.Generic;
using Zeus.Persistence.Specifications;

namespace Zeus.Collections
{
	/// <summary>
	/// Navigates a graph of content item nodes.
	/// </summary>
	public class ItemHierarchyNavigator : IHierarchyNavigator<ContentItem>
	{
		#region Fields

		private readonly HierarchyNode<ContentItem> _currentNode;

		#endregion

		#region Constructors

		public ItemHierarchyNavigator(HierarchyNode<ContentItem> currentNode)
		{
			_currentNode = currentNode;
		}

		public ItemHierarchyNavigator(HierarchyBuilder builder, params ISpecification<ContentItem>[] filters)
		{
			_currentNode = builder.Build(filters);
		}

		public ItemHierarchyNavigator(HierarchyBuilder builder)
		{
			_currentNode = builder.Build();
		}

		#endregion

		#region Properties

		public HierarchyNode<ContentItem> CurrentNode
		{
			get { return _currentNode; }
		}

		public IHierarchyNavigator<ContentItem> Parent
		{
			get
			{
				if (_currentNode.Parent != null)
					return new ItemHierarchyNavigator(_currentNode.Parent);
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

		#endregion

		#region Methods

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

		#endregion
	}
}
