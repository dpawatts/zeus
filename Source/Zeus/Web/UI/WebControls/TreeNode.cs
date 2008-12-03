using System;
using System.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public class TreeNode : Control
	{
		public bool ChildrenOnly
		{
			get;
			set;
		}

		public Control LinkControl
		{
			get;
			set;
		}

		public string AClass
		{
			get;
			set;
		}

		public string LiClass
		{
			get;
			set;
		}

		public string UlClass
		{
			get;
			set;
		}

		public ContentItem Node
		{
			get;
			set;
		}

		public TreeNode()
		{
		}

		public TreeNode(ContentItem node, Control link)
		{
			this.Node = node;
			this.LinkControl = link;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			bool isRoot = Parent is TreeNode == false;

			if (ChildrenOnly)
			{
				base.RenderChildren(writer);
			}
			else
			{
				RenderBeginTag(writer, isRoot);
				RenderChildren(writer);
				RenderEndTag(writer, isRoot);
			}
		}

		protected override void RenderChildren(HtmlTextWriter writer)
		{
			if (Controls.Count > 0)
			{
				if (string.IsNullOrEmpty(UlClass))
					writer.Write("<ul>");
				else
					writer.Write("<ul class=\"{0}\">", UlClass);

				base.RenderChildren(writer);

				writer.Write("</ul>");
			}
		}

		protected void RenderBeginTag(HtmlTextWriter writer, bool isRoot)
		{
			if (isRoot)
				writer.Write("<ul class=\"simpleTree\">");

			if (isRoot)
				writer.Write("<li id=\"{0}\" class=\"root\">", this.Node.ID);
			else if (string.IsNullOrEmpty(LiClass))
				writer.Write("<li id=\"{0}\">", this.Node.ID);
			else
				writer.Write("<li id=\"{0}\" class=\"{1}\">", this.Node.ID, LiClass);

			this.LinkControl.RenderControl(writer);
		}

		protected void RenderEndTag(HtmlTextWriter writer, bool isRoot)
		{
			writer.Write("</li>");
			if (isRoot)
				writer.Write("</ul>");
		}
	}
}