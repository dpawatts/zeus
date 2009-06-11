using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Zeus.Collections;
using Zeus.Linq;
using System.Web.Compilation;

namespace Zeus.Web.UI.WebControls
{
	/// <summary>
	/// A web control that emits a nested list of ul's and li's.
	/// </summary>
	[PersistChildren(false), ParseChildren(true)]
	public class Menu : WebControl, IContentTemplate
	{
		public event EventHandler<MenuItemCreatingEventArgs> MenuItemCreating;

		private ContentItem startPage;
		private ContentItem currentItem = null;

		public Menu()
		{
			Filter = items => items.Navigable();
		}

		#region Properties

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

		[Themeable(true)]
		public string TrailCssClass
		{
			get { return ViewState["TrailCssClass"] as string ?? "trail"; }
			set { ViewState["TrailCssClass"] = value; }
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

		private Func<IEnumerable<ContentItem>, IEnumerable<ContentItem>> Filter { get; set; }

		[PersistenceMode(PersistenceMode.InnerProperty), Browsable(false), TemplateContainer(typeof(MenuItem)), DefaultValue((string) null)]
		public ITemplate ListItemTemplate { get; set; }

		#endregion

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
			//if (!string.IsNullOrEmpty(this.OfType))
			//this.Filters.Add(new TypeSpecification<ContentItem>(BuildManager.GetType(this.OfType, true)));

			throw new NotImplementedException();
			//this.Filters.Add(new VisibleSpecification<ContentItem>());

			if (currentItem == null)
				currentItem = startPage;

			IEnumerable<ContentItem> children = currentItem.GetChildren();
			if (children.Any())
				currentItem = children.First();
			IEnumerable<ContentItem> ancestors = GetAncestors(currentItem, startPage);
			ContentItem startingPoint = GetStartingPoint();
			if (startingPoint != null)
			{
				ItemHierarchyNavigator navigator;
				if (BranchMode)
					navigator = new ItemHierarchyNavigator(new BranchHierarchyBuilder(currentItem, startingPoint), Filter);
				else
					navigator = new ItemHierarchyNavigator(new TreeHierarchyBuilder(startingPoint, MaxLevels), Filter);
				if (navigator.Current != null)
					AddControlsRecursive(this, navigator, CurrentPage, ancestors);
			}
		}

		private void AddControlsRecursive(Control container, IHierarchyNavigator<ContentItem> ih, ContentItem selectedPage, IEnumerable<ContentItem> ancestors)
		{
			foreach (ItemHierarchyNavigator childHierarchy in ih.Children)
			{
				if (!(childHierarchy.Current is ContentItem))
					continue;

				ContentItem current = (ContentItem) childHierarchy.Current;

				string cssClass = string.Empty;
				if (current == selectedPage || current.Url == selectedPage.Url)
					cssClass = this.SelectedCssClass;
				else if (ancestors.Contains(current))
					cssClass = TrailCssClass;

				Control itemPlaceholder;
				if (ListItemTemplate != null)
				{
					MenuItem control = new MenuItem();
					control.DataItem = current;
					control.DataItemCssClass = cssClass;
					ListItemTemplate.InstantiateIn(control);
					container.Controls.Add(control);
					control.DataBind();
					itemPlaceholder = control.FindControl("itemPlaceholder");
				}
				else
				{
					itemPlaceholder = CreateAndAdd(container, "li", null);
					((HtmlGenericControl) itemPlaceholder).Attributes["class"] = cssClass;
				}

				string url = current.Url;
				if (MenuItemCreating != null)
				{
					MenuItemCreatingEventArgs args = new MenuItemCreatingEventArgs { CurrentItem = current, Url = current.Url };
					MenuItemCreating(this, args);
					url = args.Url;
				}
				HtmlAnchor anchor = new HtmlAnchor();
				anchor.HRef = url;
				anchor.InnerText = current.Title;
				itemPlaceholder.Controls.Add(anchor);

				HtmlGenericControl ul = new HtmlGenericControl("ul");
				AddControlsRecursive(ul, childHierarchy, selectedPage, ancestors);
				if (ul.Controls.Count > 0)
					itemPlaceholder.Controls.Add(ul);
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

	public class MenuItemCreatingEventArgs : EventArgs
	{
		public ContentItem CurrentItem { get; internal set; }
		public string Url { get; set; }
	}
}
