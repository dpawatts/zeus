using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Isis.Web;
using Zeus.Web.UI.WebControls;
using Zeus.Linq.Filters;

namespace Zeus.Admin.Navigation
{
	public class TreeLoader : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";

			string path = context.Request.GetRequiredString("selected");
			ContentItem selectedItem = Zeus.Context.Current.Resolve<Navigator>().Navigate(path);

			ItemFilter filter = new AccessFilter(context.User, Zeus.Context.SecurityManager);
			if (context.User.Identity.Name != "administrator")
				filter = new CompositeFilter(new PageFilter(), filter);
			TreeNode tree = Zeus.Web.Tree.From(selectedItem, 2)
				.LinkProvider(BuildLink)
				.Filters(filter)
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
			anchor.Attributes["data-url"] = Zeus.Web.Url.ToAbsolute(node.Url);

			HtmlImage image = new HtmlImage();
			image.Src = node.IconUrl;
			anchor.Controls.Add(image);
			anchor.Controls.Add(new LiteralControl(node.Title));

			HtmlGenericControl span = new HtmlGenericControl("span");
			span.ID = "span" + node.ID;
			span.Attributes["data-path"] = node.Path;
			span.Attributes["data-type"] = node.GetType().Name;
			span.Controls.Add(anchor);

			return span;
		}

		public bool IsReusable
		{
			get { return false; }
		}
	}
}
