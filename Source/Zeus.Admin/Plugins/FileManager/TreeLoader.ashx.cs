using System;
using System.Linq;
using System.Web;
using Ext.Net;
using Zeus.Admin.Plugins.Tree;
using Zeus.Linq;
using Zeus.Security;
using Zeus.Web;

namespace Zeus.Admin.Plugins.FileManager
{
	public class TreeLoader : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/json";
			int? nodeId = !string.IsNullOrEmpty(context.Request["node"]) ? Convert.ToInt32(context.Request["node"]) as int? : null;

			if (nodeId != null)
			{
				ContentItem selectedItem = Context.Persister.Get(nodeId.Value);

				SiteTree tree = SiteTree.From(selectedItem.TranslationOf ?? selectedItem, 2);

				TreeNodeBase treeNode = tree.Filter(items => items.Authorized(context.User, Context.SecurityManager, Operations.Read).Where(ci => !(ci is WidgetContentItem)))
					.ToTreeNode(false, false);

				if (treeNode is TreeNode)
				{
					string json = ((TreeNode) treeNode).Nodes.ToJson();
					context.Response.Write(json);
					context.Response.End();
				}
			}
		}

		public bool IsReusable
		{
			get { return false; }
		}
	}
}