using System;
using System.Web.UI.WebControls;
using System.Web;

namespace Isis.Web.UI.WebControls
{
	public class ExtendedTreeView : TreeView
	{
		protected override void OnTreeNodeDataBound(TreeNodeEventArgs e)
		{
			base.OnTreeNodeDataBound(e);

			string tempVisibleInMenu = ((SiteMapNode) e.Node.DataItem)["visibleInMenu"];
			if (!string.IsNullOrEmpty(tempVisibleInMenu))
			{
				if (!Convert.ToBoolean(tempVisibleInMenu))
					e.Node.Parent.ChildNodes.Remove(e.Node);
			}
		}
	}
}
