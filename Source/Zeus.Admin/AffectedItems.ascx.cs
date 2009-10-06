using System;
using System.Web.UI.HtmlControls;
using Isis.Web;
using Zeus.Web.UI;

namespace Zeus.Admin
{
	public partial class AffectedItems : System.Web.UI.UserControl, IContentTemplate
	{
		protected override void OnDataBinding(EventArgs e)
		{
			base.OnPreRender(e);

			Controls.Clear();
			HtmlGenericControl ul = new HtmlGenericControl("ul");
			Controls.Add(ul);
			AddChildrenRecursive(CurrentItem, ul);
		}

		private static void AddChildrenRecursive(ContentItem item, HtmlGenericControl container)
		{
			HtmlGenericControl li = new HtmlGenericControl("li");
			li.InnerHtml = string.Format("<a href='{0}'><img src='{1}'/>{2}</a>", Url.ToAbsolute(((INode) item).PreviewUrl), Url.ToAbsolute(item.IconUrl), item.Title);
			container.Controls.Add(li);

			if (item.Children.Count > 0)
			{
				HtmlGenericControl ul = new HtmlGenericControl("ul");
				li.Controls.Add(ul);

				foreach (ContentItem child in item.Children)
					AddChildrenRecursive(child, ul);
			}
		}

		public ContentItem CurrentItem { get; set; }
	}
}