using System;
using System.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class Tree : Control
	{
		private IContentItem _selectedItem = null;

		private IContentItem StartNode
		{
			get
			{
				using (GibbonsDataContext db = GibbonsDataContext.Open())
				{
					return db.StartPage;
				}
			}
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			string selected = Request.QueryString["selected"];
			if (!string.IsNullOrEmpty(selected))
				_selectedItem = this.StartNode.GetChild(selected);
			else
				_selectedItem = this.StartNode;

			Web.Tree tree = Web.Tree.Between(_selectedItem, this.StartNode, true)
				.OpenTo(_selectedItem)
				.LinkProvider(BuildLink)
				.ToControl();

			AppendExpanderNodeRecursive(tree);

			tree.LiClass = "root";

			plcTree.Controls.Add(tree);
		}

		public static void AppendExpanderNodeRecursive(Control tree)
		{
			TreeNode tn = tree as TreeNode;
			if (tn != null)
			{
				foreach (Control child in tn.Controls)
					AppendExpanderNodeRecursive(child);
				if (tn.Controls.Count == 0 && tn.Node.GetChildren().Count() > 0)
					AppendExpanderNode(tn);
			}
		}

		public static void AppendExpanderNode(TreeNode tn)
		{
			HtmlGenericControl li = new HtmlGenericControl("li");
			li.InnerText = "{url:/Admin/Navigation/TreeLoader.ashx?selected=" + HttpUtility.UrlEncode(tn.Node.GetPath()) + "}";

			tn.UlClass = "ajax";
			tn.Controls.Add(li);
		}

		private Control BuildLink(IContentItem node)
		{
			HtmlAnchor anchor = new HtmlAnchor();
			anchor.HRef = node.GetUrl();
			anchor.Target = "preview";

			HtmlImage image = new HtmlImage();
			image.Src = node.IconUrl;
			anchor.Controls.Add(image);
			anchor.Controls.Add(new LiteralControl(node.Title));

			HtmlGenericControl span = new HtmlGenericControl("span");
			span.ID = "span" + node.GetType().FullName.Replace(".", string.Empty) + node.ID;
			span.Attributes["data-adminurl"] = node.AdminUrl;
			span.Attributes["data-type"] = node.GetType().FullName;
			span.Attributes["data-id"] = node.ID.ToString();
			if (node.GetType() == _selectedItem.GetType() && node.ID == _selectedItem.ID)
				span.Attributes["class"] = "active";
			span.Controls.Add(anchor);

			return span;
		}
	}
}
