using System;
using System.Linq;
using Zeus.Collections;
using System.Web.UI;
using System.Collections.Generic;
using Zeus.Web.UI.WebControls;

namespace Zeus.Web
{
	public class Tree
	{
		public delegate string ClassProviderDelegate(ContentItem currentItem);
		public delegate Control LinkProviderDelegate(ContentItem currentItem);

		private HierarchyBuilder _treeBuilder;
		private ClassProviderDelegate _classProvider = delegate { return string.Empty; };
		private LinkProviderDelegate _linkProvider;
		private bool _excludeRoot;

		public Tree(HierarchyBuilder treeBuilder)
		{
			_treeBuilder = treeBuilder;
		}

		public static Tree From(ContentItem rootItem)
		{
			return new Tree(new TreeHierarchyBuilder(rootItem));
		}

		public static Tree From(ContentItem rootItem, int maxDepth)
		{
			return new Tree(new TreeHierarchyBuilder(rootItem, maxDepth));
		}

		public static Tree Between(ContentItem initialItem, ContentItem lastAncestor, bool appendAdditionalLevel)
		{
			return new Tree(new BranchHierarchyBuilder(initialItem, lastAncestor, appendAdditionalLevel));
		}

		public Tree ClassProvider(ClassProviderDelegate classProvider)
		{
			_classProvider = classProvider;
			return this;
		}

		public Tree ExcludeRoot(bool exclude)
		{
			_excludeRoot = exclude;
			return this;
		}

		public Tree LinkProvider(LinkProviderDelegate linkProvider)
		{
			_linkProvider = linkProvider;
			return this;
		}

		public Tree OpenTo(ContentItem item)
		{
			IList<ContentItem> items = Find.ListParents(item);
			return ClassProvider(c => (items.Contains(c) || c == item) ? "open" : string.Empty);
		}

		public TreeNode ToControl()
		{
			IHierarchyNavigator<ContentItem> navigator = new ItemHierarchyNavigator(_treeBuilder);
			TreeNode rootNode = BuildNodesRecursive(navigator);
			rootNode.ChildrenOnly = _excludeRoot;
			return rootNode;
		}

		private TreeNode BuildNodesRecursive(IHierarchyNavigator<ContentItem> navigator)
		{
			ContentItem item = navigator.Current;

			TreeNode node = new TreeNode(item, _linkProvider(item));
			node.LiClass = _classProvider(item);

			foreach (IHierarchyNavigator<ContentItem> childNavigator in navigator.Children)
			{
				TreeNode childNode = BuildNodesRecursive(childNavigator);
				node.Controls.Add(childNode);
			}
			return node;
		}
	}
}