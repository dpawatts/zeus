using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Isis.Web.UI.WebControls.Adapters;

namespace Isis.Web.UI.WebControls
{
	public class TreeView : System.Web.UI.WebControls.TreeView
	{
		private TreeNodeStyle _selectedNodeParentsStyle;

		#region Properties

		public string ActualID
		{
			get { return ViewState["ActualID"] as string ?? string.Empty; }
			set { ViewState["ActualID"] = value; }
		}

		[PersistenceMode(PersistenceMode.InnerProperty), Browsable(false), TemplateContainer(typeof(TreeViewTemplateContainer)), DefaultValue((string) null)]
		public ITemplate ItemTemplate
		{
			get;
			set;
		}

		[DefaultValue((string) null), PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true)]
		public TreeNodeStyle SelectedNodeParentsStyle
		{
			get
			{
				if (_selectedNodeParentsStyle == null)
				{
					_selectedNodeParentsStyle = new TreeNodeStyle();
					if (base.IsTrackingViewState)
						((IStateManager) _selectedNodeParentsStyle).TrackViewState();
				}
				return _selectedNodeParentsStyle;
			}
		}

		[PersistenceMode(PersistenceMode.InnerProperty), Browsable(false), TemplateContainer(typeof(TreeViewTemplateContainer)), DefaultValue((string) null)]
		public ITemplate RootItemTemplate
		{
			get;
			set;
		}

		[DefaultValue(-1)]
		public int RenderDepth
		{
			get { return (int) (ViewState["RenderDepth"] ?? -1); }
			set { ViewState["RenderDepth"] = value; }
		}

		#endregion

		#region Viewstate management

		protected override void LoadViewState(object state)
		{
			if (state != null)
			{
				Pair pair = (Pair) state;
				if (pair.First != null)
					base.LoadViewState(pair.First);
				if (pair.Second != null)
					((IStateManager) SelectedNodeParentsStyle).LoadViewState(pair.Second);
			}
		}

		protected override object SaveViewState()
		{
			Pair pair = new Pair();
			pair.First = base.SaveViewState();
			if (_selectedNodeParentsStyle != null)
				pair.Second = ((IStateManager) _selectedNodeParentsStyle).SaveViewState();
			return (pair.First != null || pair.Second != null) ? pair : null;
		}

		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (_selectedNodeParentsStyle != null)
				((IStateManager) _selectedNodeParentsStyle).TrackViewState();
		}

		#endregion

		protected override void OnTreeNodeDataBound(TreeNodeEventArgs e)
		{
			base.OnTreeNodeDataBound(e);

			SiteMapNode siteMapNode = (SiteMapNode) e.Node.DataItem;
			if (siteMapNode["linkToFirstChild"] == "true" && siteMapNode.ChildNodes.Count > 0)
				e.Node.NavigateUrl = siteMapNode.ChildNodes[0].Url;
		}

		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			writer.WriteLine();
			writer.Indent++;
			BuildItems(Nodes, 0, writer);
			writer.Indent--;
			writer.WriteLine();
		}

		public override void RenderEndTag(HtmlTextWriter writer)
		{
			
		}

		private void BuildItems(TreeNodeCollection items, int depth, HtmlTextWriter writer)
		{
			if (items.Count <= 0)
				return;

			writer.WriteLine();
			writer.WriteBeginTag("ul");

			if (depth == 0)
			{
				if (!string.IsNullOrEmpty(ActualID))
					writer.WriteAttribute("id", ActualID);

				if (!string.IsNullOrEmpty(CssClass))
					writer.WriteAttribute("class", CssClass);
			}

			writer.Write(HtmlTextWriter.TagRightChar);
			writer.Indent++;

			foreach (TreeNode item in items)
				BuildItem(item, depth, writer);

			writer.Indent--;
			writer.WriteLine();
			writer.WriteEndTag("ul");
		}

		private void BuildItem(TreeNode item, int depth, HtmlTextWriter writer)
		{
			if (item == null || writer == null)
				return;

			writer.WriteLine();
			writer.WriteBeginTag("li");

			string cssClass = GetNodeClass(item);
			if (!string.IsNullOrEmpty(cssClass))
				writer.WriteAttribute("class", cssClass);
			writer.Write(HtmlTextWriter.TagRightChar);

			if (HasChildren(item))
			{
				writer.Indent++;
				writer.WriteLine();
			}

			ITemplate template = ItemTemplate;
			if (depth == 0)
				template = RootItemTemplate;

			if (template != null)
			{
				TreeViewTemplateContainer tempControl = new TreeViewTemplateContainer();
				template.InstantiateIn(tempControl);

				tempControl.DataItem = item;
				tempControl.DataBind();

				tempControl.RenderControl(writer);
			}
			else
			{
				if (IsLink(item))
					WriteNodeLink(item, writer);
				else
					WriteNodePlain(item, writer);
			}

			if (HasChildren(item) && (RenderDepth == -1 || depth < RenderDepth))
			{
				BuildItems(item.ChildNodes, depth + 1, writer);

				writer.Indent--;
				writer.WriteLine();
			}

			writer.WriteEndTag("li");
		}

		private void WriteNodeLink(TreeNode item, HtmlTextWriter writer)
		{
			writer.WriteBeginTag("a");

			if (!string.IsNullOrEmpty(item.NavigateUrl))
				writer.WriteAttribute("href", ResolveUrl(item.NavigateUrl));

			WebControlAdapterExtender.WriteTargetAttribute(writer, item.Target);

			if (!string.IsNullOrEmpty(item.ToolTip))
				writer.WriteAttribute("title", item.ToolTip);
			else if (!string.IsNullOrEmpty(ToolTip))
				writer.WriteAttribute("title", ToolTip);

			writer.Write(HtmlTextWriter.TagRightChar);
			writer.Write(item.Text);
			writer.WriteEndTag("a");
		}

		private static void WriteNodePlain(TreeNode item, HtmlTextWriter writer)
		{
			writer.Write(item.Text);
		}

		private string GetNodeClass(TreeNode item)
		{
			string value = string.Empty;
			if (item != null)
				if (item.Selected)
					value = SelectedNodeStyle.CssClass;
				else if (IsChildNodeSelected(item))
					value += SelectedNodeParentsStyle.CssClass;
			return value;
		}

		private static bool IsChildNodeSelected(TreeNode item)
		{
			if (item != null && item.ChildNodes != null)
				foreach (TreeNode childNode in item.ChildNodes)
					if (childNode.Selected)
						return true;

			return false;
		}

		private static bool IsLink(TreeNode item)
		{
			return item != null && (!string.IsNullOrEmpty(item.NavigateUrl));
		}

		private static bool HasChildren(TreeNode item)
		{
			return ((item != null) && ((item.ChildNodes != null) && (item.ChildNodes.Count > 0)));
		}

		private new string ResolveUrl(string url)
		{
			string urlToResolve = url;
			int nPound = url.LastIndexOf("#");
			int nSlash = url.LastIndexOf("/");
			if ((nPound > -1) && (nSlash > -1) && ((nSlash + 1) == nPound))
			{
				//  We have been given a somewhat strange URL.  It has a foreward slash (/) immediately followed
				//  by a pound sign (#) like this xxx/#yyy.  This sort of oddly shaped URL is what you get when
				//  you use named anchors instead of pages in the url attribute of a sitemapnode in an ASP.NET
				//  sitemap like this:
				//
				//  <siteMapNode url="#Introduction" title="Introduction"  description="Introduction" />
				//
				//  The intend of the sitemap author is clearly to create a link to a named anchor in the page
				//  that looks like these:
				//
				//  <a id="Introduction"></a>       (XHTML 1.1 Strict compliant)
				//  <a name="Introduction"></a>     (more old fashioned but quite common in many pages)
				//
				//  However, the sitemap interpretter in ASP.NET doesn't understand url values that start with
				//  a pound.  It prepends the current site's path in front of it before making it into a link
				//  (typically for a TreeView or Menu).  We'll undo that problem, however, by converting this
				//  sort of oddly shaped URL back into what was intended: a simple reference to a named anchor
				//  that is expected to be within the current page.

				urlToResolve = url.Substring(nPound);
			}
			else
			{
				urlToResolve = ResolveClientUrl(urlToResolve);
			}

			//  And, just to be safe, we'll make sure there aren't any troublesome characters in whatever URL
			//  we have decided to use at this point.
			string newUrl = this.Page.Server.HtmlEncode(urlToResolve);

			return newUrl;
		}
	}
}
