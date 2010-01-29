using System;
using System.Linq;
using System.Web;
using Ext.Net;
using Zeus.Linq;
using Zeus.Security;
using Zeus.Web;

namespace Zeus.Admin.Plugins.Tree
{
	public class TreeLoader : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/json";
			string fromRootTemp = context.Request["fromRoot"];
			bool fromRoot = false;
			if (!string.IsNullOrEmpty(fromRootTemp))
				fromRoot = Convert.ToBoolean(fromRootTemp);
			bool sync = (context.Request["sync"] == "true");
			int? nodeId = !string.IsNullOrEmpty(context.Request["node"]) ? Convert.ToInt32(context.Request["node"]) as int? : null;
			int? overrideNodeId = !string.IsNullOrEmpty(context.Request["overrideNode"]) ? Convert.ToInt32(context.Request["overrideNode"]) as int? : null;

			nodeId = overrideNodeId ?? nodeId;
			if (nodeId != null)
			{
				ContentItem selectedItem = Context.Persister.Get(nodeId.Value);

				SiteTree tree;
				if (sync)
					tree = SiteTree.From(selectedItem, int.MaxValue);
				else if (fromRoot)
					tree = SiteTree.Between(selectedItem, Find.RootItem, true)
						.OpenTo(selectedItem);
				else
					tree = SiteTree.From(selectedItem.TranslationOf ?? selectedItem, 2);

				if (sync)
					tree = tree.ForceSync();

				//if (context.User.Identity.Name != "administrator")
				//	filter = new CompositeSpecification<ContentItem>(new PageSpecification<ContentItem>(), filter);
				TreeNodeBase treeNode = tree.Filter(items => items.Authorized(context.User, Context.SecurityManager, Operations.Read).Where(ci => !(ci is WidgetContentItem)))
					.ToTreeNode(false);

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