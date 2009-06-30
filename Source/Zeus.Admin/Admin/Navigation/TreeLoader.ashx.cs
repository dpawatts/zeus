using System.Web;
using System.Web.UI;
using Isis.ExtensionMethods.Web;
using Isis.Web.Hosting;
using Zeus.Linq;
using Zeus.Security;
using Zeus.Web.UI.WebControls;

[assembly: EmbeddedResourceFile("Zeus.Admin.Navigation.TreeLoader.ashx", "Zeus.Admin")]
namespace Zeus.Admin.Navigation
{
	public class TreeLoader : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";

			string path = context.Request.GetRequiredString("selected");
			ContentItem selectedItem = Zeus.Context.Current.Resolve<Navigator>().Navigate(path);

			//if (context.User.Identity.Name != "administrator")
			//	filter = new CompositeSpecification<ContentItem>(new PageSpecification<ContentItem>(), filter);
			TreeNode tree = Zeus.Web.Tree.From(selectedItem.TranslationOf ?? selectedItem, 2)
				.LinkProvider(BuildLink)
				.Filter(items => items.Authorized(context.User, Context.SecurityManager, Operations.Read))
				.ToControl();

			Web.UI.WebControls.Tree.AppendExpanderNodeRecursive(tree);

			using (HtmlTextWriter writer = new HtmlTextWriter(context.Response.Output))
				foreach (Control childControl in tree.Controls)
					childControl.RenderControl(writer);
		}

		private static Control BuildLink(ContentItem node)
		{
			return Web.UI.WebControls.Tree.BuildLink(node, null);
		}

		public bool IsReusable
		{
			get { return false; }
		}
	}
}
