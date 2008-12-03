using System;
using System.Collections.Generic;

namespace Zeus.Collections
{
	public class HierarchyNode<T>
	{
		private List<HierarchyNode<T>> _children;

		public HierarchyNode(T current)
		{
			this.Current = current;
			_children = new List<HierarchyNode<T>>();
		}

		/// <summary>
		/// Gets or sets the current node value.
		/// </summary>
		public T Current
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the parent node.
		/// </summary>
		public HierarchyNode<T> Parent
		{
			get;
			set;
		}

		public IList<HierarchyNode<T>> Children
		{
			get { return _children; }
		}
	}
}
