using System;
using System.Linq;
using System.Web.UI;
using Zeus.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web;
using Isis.Web;

namespace Zeus.Admin.Web.UI.WebControls
{
	public class Tree : Control
	{
		private ContentItem _selectedItem = null, _rootItem = null;

		public ContentItem SelectedItem
		{
			get { return _selectedItem ?? (_selectedItem = Find.CurrentPage ?? Find.StartPage); }
			set { _selectedItem = value; }
		}

		public ContentItem RootNode
		{
			get { return _rootItem ?? Find.RootItem; }
			set { _rootItem = value; }
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			string path = this.Page.Request.GetOptionalString("selected");
			_selectedItem = (!string.IsNullOrEmpty(path)) ? Zeus.Context.Current.Resolve<Navigator>().Navigate(path) : Find.RootItem;

			TreeNode treeNode = Zeus.Web.Tree.Between(_selectedItem, this.RootNode, true)
				.OpenTo(_selectedItem)
				.LinkProvider(BuildLink)
				.ToControl();

			AppendExpanderNodeRecursive(treeNode);

			treeNode.LiClass = "root";

			this.Controls.Add(treeNode);
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
			li.InnerText = "{url:/Admin/Navigation/TreeLoader.ashx?selected=" + HttpUtility.UrlEncode(tn.Node.Path) + "}";

			tn.UlClass = "ajax";
			tn.Controls.Add(li);
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
			span.Attributes["data-path"] = node.Path;
			if (node.Path == _selectedItem.Path)
				span.Attributes["class"] = "active";
			span.Controls.Add(anchor);

			return span;
		}
	}
}
