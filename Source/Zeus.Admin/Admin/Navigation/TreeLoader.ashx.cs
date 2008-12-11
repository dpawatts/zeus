using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Isis.Web;
using Zeus.Web.UI.WebControls;

namespace Zeus.Admin.Navigation
{
	public class TreeLoader : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";

			int? itemID = context.Request.GetOptionalInt("selecteditem");
			ContentItem selectedItem = (itemID != null) ? Zeus.Context.Persister.Get(itemID.Value) : Find.RootItem;

			TreeNode tree = Zeus.Web.Tree.From(selectedItem, 2)
				.LinkProvider(BuildLink)
				.ToControl();

			Zeus.Admin.Web.UI.WebControls.Tree.AppendExpanderNodeRecursive(tree);

			using (HtmlTextWriter writer = new HtmlTextWriter(context.Response.Output))
			{
				foreach (Control childControl in tree.Controls)
					childControl.RenderControl(writer);
			}
		}

		private Control BuildLink(ContentItem node)
		{
			HtmlAnchor anchor = new HtmlAnchor();
			anchor.HRef = node.Url;
			anchor.Target = "preview";

			HtmlImage image = new HtmlImage();
			image.Src = node.IconUrl;
			anchor.Controls.Add(image);
			anchor.Controls.Add(new LiteralControl(node.Title));

			HtmlGenericControl span = new HtmlGenericControl("span");
			span.ID = "span" + node.ID;
			span.Attributes["data-id"] = node.ID.ToString();
			span.Controls.Add(anchor);

			return span;
		}

		public bool IsReusable
		{
			get { return false; }
		}
	}
}
