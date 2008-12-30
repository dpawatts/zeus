using System;
using System.Linq;
using Zeus.Linq.Filters;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Zeus.Collections;
using System.Web.Compilation;

namespace Zeus.Web.UI.WebControls
{
	/// <summary>
	/// A web control that emits a nested list of ul's and li's.
	/// </summary>
	public class Menu : WebControl, IContentTemplate
	{
		private ContentItem startPage;
		private ContentItem currentItem = null;

		public Menu()
		{
			this.Filters = new List<ItemFilter>(new ItemFilter[] { new NavigationFilter() });
		}

		public virtual ContentItem CurrentItem
		{
			get { return currentItem ?? (currentItem = this.FindCurrentItem()); }
			set { currentItem = value; }
		}

		public ContentItem StartPage
		{
			get { return (startPage ?? (startPage = Find.StartPage)); }
		}

		[Themeable(true)]
		public int StartLevel
		{
			get { return (int) (ViewState["StartLevel"] ?? 1); }
			set { ViewState["StartLevel"] = value; }
		}

		[Themeable(true)]
		public bool BranchMode
		{
			get { return (bool) (ViewState["BranchMode"] ?? true); }
			set { ViewState["BranchMode"] = value; }
		}

		[Themeable(true)]
		public int MaxLevels
		{
			get { return (int) (ViewState["MaxLevels"] ?? int.MaxValue); }
			set { ViewState["MaxLevels"] = value; }
		}

		[Themeable(true)]
		public string SelectedCssClass
		{
			get { return ViewState["SelectedCssClass"] as string ?? "current"; }
			set { ViewState["SelectedCssClass"] = value; }
		}

		public string OfType
		{
			get { return ViewState["OfType"] as string ?? string.Empty; }
			set { ViewState["OfType"] = value; }
		}

		protected override HtmlTextWriterTag TagKey
		{
			get { return HtmlTextWriterTag.Ul; }
		}

		private IList<ItemFilter> Filters
		{
			get;
			set;
		}

		#region Methods

		protected override void CreateChildControls()
		{
			Page.Trace.Write("Create Menu controls - Begin");
			BuildControlHierarchy(this.CurrentPage, this.StartPage);
			base.CreateChildControls();
			Page.Trace.Write("Create Menu controls - End");
		}

		private static HtmlGenericControl CreateAndAdd(Control container, string tagName, string cssClass)
		{
			HtmlGenericControl hgc = new HtmlGenericControl(tagName);
			if (!string.IsNullOrEmpty(cssClass))
				hgc.Attributes["class"] = cssClass;
			container.Controls.Add(hgc);
			return hgc;
		}

		private void BuildControlHierarchy(ContentItem currentItem, ContentItem startPage)
		{
			if (!string.IsNullOrEmpty(this.OfType))
				this.Filters.Add(new TypeFilter(BuildManager.GetType(this.OfType, true)));

			this.Filters.Add(new VisibleFilter());

			if (currentItem == null)
				currentItem = startPage;

			IList<ContentItem> children = currentItem.GetChildren();
			if (children.Count > 0)
				currentItem = children[0];
			IEnumerable<ContentItem> ancestors = GetAncestors(currentItem, startPage);
			ContentItem startingPoint = GetStartingPoint();
			if (startingPoint != null)
			{
				ItemHierarchyNavigator navigator;
				if (BranchMode)
					navigator = new ItemHierarchyNavigator(new BranchHierarchyBuilder(currentItem, startingPoint), this.Filters.ToArray());
				else
					navigator = new ItemHierarchyNavigator(new TreeHierarchyBuilder(startingPoint, MaxLevels), this.Filters.ToArray());
				if (navigator.Current != null)
					AddControlsRecursive(this, navigator, this.CurrentPage, ancestors);
			}
		}

		private void AddControlsRecursive(Control container, IHierarchyNavigator<ContentItem> ih, ContentItem selectedPage, IEnumerable<ContentItem> ancestors)
		{
			foreach (ItemHierarchyNavigator childHierarchy in ih.Children)
			{
				if (!childHierarchy.Current.IsPage)
					continue;

				HtmlGenericControl li = CreateAndAdd(container, "li", null);

				ContentItem current = childHierarchy.Current;
				HtmlAnchor anchor = new HtmlAnchor();
				anchor.HRef = current.Url;
				anchor.InnerText = current.Title;
				li.Controls.Add(anchor);

				if (current == selectedPage || current.Url == selectedPage.Url)
					li.Attributes["class"] = this.SelectedCssClass;
				else if (ancestors.Contains(current))
					li.Attributes["class"] = "trail";

				HtmlGenericControl ul = new HtmlGenericControl("ul");
				AddControlsRecursive(ul, childHierarchy, selectedPage, ancestors);
				if (ul.Controls.Count > 0)
					li.Controls.Add(ul);
			}
		}

		private ContentItem GetStartingPoint()
		{
			return Find.AncestorAtLevel(this.StartLevel, Find.EnumerateParents(this.CurrentPage, this.StartPage, true), this.CurrentPage);
		}

		private static IEnumerable<ContentItem> GetAncestors(ContentItem currentItem, ContentItem startPage)
		{
			return Find.EnumerateParents(currentItem, startPage);
		}

		#endregion [rgn]

		public ContentItem CurrentPage
		{
			get { return Find.CurrentPage; }
		}
	}
}
