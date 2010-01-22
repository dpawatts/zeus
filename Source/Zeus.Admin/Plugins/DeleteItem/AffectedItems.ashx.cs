using System;
using System.Linq;
using System.Web;
using Ext.Net;
using Zeus.Admin.Plugins.Tree;
using Zeus.Linq;
using Zeus.Security;

namespace Zeus.Admin.Plugins.DeleteItem
{
	public class AffectedItems : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/json";

			// "node" will be a comma-separated list of nodes that are going to be deleted.
			string node = context.Request["node"];

			if (!string.IsNullOrEmpty(node))
			{
				string[] nodeIDsTemp = node.Split(',');
				var nodeIDs = nodeIDsTemp.Select(s => Convert.ToInt32(s));

				TreeNodeCollection treeNodes = new TreeNodeCollection();

				foreach (int nodeID in nodeIDs)
				{
					ContentItem selectedItem = Context.Persister.Get(nodeID);

					SiteTree tree = SiteTree.From(selectedItem, int.MaxValue);

					TreeNodeBase treeNode = tree.Filter(items => items.Authorized(context.User, Context.SecurityManager, Operations.Read))
						.ToTreeNode(false);

					treeNodes.Add(treeNode);
				}

				string json = treeNodes.ToJson();
				context.Response.Write(json);
				context.Response.End();
			}
		}

		public bool IsReusable
		{
			get { return false; }
		}
	}
}