using System;
using System.Linq;
using Coolite.Ext.Web;
using Zeus.BaseLibrary.Web;
using Zeus.Collections;
using System.Collections.Generic;

namespace Zeus.Admin.Plugins.Tree
{
	public class SiteTree
	{
		private readonly HierarchyBuilder _treeBuilder;
		private Func<IEnumerable<ContentItem>, IEnumerable<ContentItem>> _filter;
		private bool _excludeRoot, _forceSync;

		public SiteTree(HierarchyBuilder treeBuilder)
		{
			_treeBuilder = treeBuilder;
		}

		public static SiteTree From(ContentItem rootItem)
		{
			return new SiteTree(new TreeHierarchyBuilder(rootItem));
		}

		public static SiteTree From(ContentItem rootItem, int maxDepth)
		{
			return new SiteTree(new TreeHierarchyBuilder(rootItem, maxDepth));
		}

		public static SiteTree Between(ContentItem initialItem, ContentItem lastAncestor, bool appendAdditionalLevel)
		{
			return new SiteTree(new BranchHierarchyBuilder(initialItem, lastAncestor, appendAdditionalLevel));
		}

		public SiteTree ExcludeRoot(bool exclude)
		{
			_excludeRoot = exclude;
			return this;
		}

		public SiteTree Filter(Func<IEnumerable<ContentItem>, IEnumerable<ContentItem>> filter)
		{
			_filter = filter;
			return this;
		}

		public SiteTree ForceSync()
		{
			_forceSync = true;
			return this;
		}

		public SiteTree OpenTo(ContentItem item)
		{
			IList<ContentItem> items = Find.ListParents(item);
			//return ClassProvider(c => (items.Contains(c) || c == item) ? "open" : string.Empty);
			return this;
		}

		public TreeNodeBase ToTreeNode(bool rootOnly)
		{
			IHierarchyNavigator<ContentItem> navigator = new ItemHierarchyNavigator(_treeBuilder, _filter);
			TreeNodeBase rootNode = BuildNodesRecursive(navigator, rootOnly);
			//rootNode.ChildrenOnly = _excludeRoot;
			return rootNode;
		}

		private TreeNodeBase BuildNodesRecursive(IHierarchyNavigator<ContentItem> navigator, bool rootOnly)
		{
			ContentItem item = navigator.Current;

			bool hasAsyncChildren = ((!navigator.Children.Any() && item.GetChildren().Any()) || rootOnly);
			TreeNodeBase node = (hasAsyncChildren) ? new AsyncTreeNode() as TreeNodeBase : new TreeNode();
			node.Text = ((INode) item).Contents;
			node.IconFile = item.IconUrl;
			node.IconCls = "zeus-tree-icon";
			node.Cls = "zeus-tree-node";
			node.NodeID = item.ID.ToString();
			node.Href = "javascript:window.top.zeus.refreshPreview('" + Url.ToAbsolute(((INode)item).PreviewUrl) + "');";

			if (!hasAsyncChildren)
				foreach (IHierarchyNavigator<ContentItem> childNavigator in navigator.Children)
				{
					TreeNodeBase childNode = BuildNodesRecursive(childNavigator, rootOnly);
					((TreeNode) node).Nodes.Add(childNode);
				}
			if (!item.GetChildren().Any())
			{
				node.CustomAttributes.Add(new ConfigItem("children", "[]", ParameterMode.Raw));
				node.Expanded = true;
			}
			else if (navigator.Children.Any())
				node.Expanded = true;
			return node;
		}
	}
}