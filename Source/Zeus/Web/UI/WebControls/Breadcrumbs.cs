using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	[PersistChildren(false), ParseChildren(true)]
	public class Breadcrumbs : Control
	{
		#region Properties

		[PersistenceMode(PersistenceMode.InnerProperty), Browsable(false), TemplateContainer(typeof(BreadcrumbItem)), DefaultValue((string) null)]
		public ITemplate ItemTemplate { get; set; }

		[PersistenceMode(PersistenceMode.InnerProperty), Browsable(false), TemplateContainer(typeof(BreadcrumbItem)), DefaultValue((string) null)]
		public ITemplate LastItemTemplate { get; set; }

		[PersistenceMode(PersistenceMode.InnerProperty), Browsable(false), TemplateContainer(typeof(BreadcrumbItem)), DefaultValue((string) null)]
		public ITemplate SeparatorTemplate { get; set; }

		public string SeparatorText
		{
			get { return (string) (ViewState["SeparatorText"] ?? " / "); }
			set { ViewState["SeparatorText"] = value; }
		}

		public int StartLevel
		{
			get { return (int) (ViewState["StartLevel"] ?? 1); }
			set { ViewState["StartLevel"] = value; }
		}

		/// <summary>The page depth level below which the path is hidden from view. This is used to hide the path on the start page.</summary>
		public int VisibilityLevel { get; set; }

		#endregion

		protected override void CreateChildControls()
		{
			int added = 0;
			var parents = Find.EnumerateParents(Find.CurrentPage, Find.StartPage, true);
			if (StartLevel != 1 && parents.Count() >= StartLevel)
				parents = parents.Take(parents.Count() - StartLevel);
			foreach (ContentItem page in parents)
			{
				IBreadcrumbAppearance appearance = page as IBreadcrumbAppearance;
				bool visible = appearance == null || appearance.VisibleInBreadcrumb;
				if (visible && page.IsPage)
				{
					ILink link = appearance ?? (ILink) page;
					if (added > 0)
					{
						Control separator = new Control();
						SeparatorTemplate.InstantiateIn(separator);
						Controls.AddAt(0, separator);
						Controls.AddAt(0, new LiteralControl(Environment.NewLine));
						AddBreadcrumbItem(link, ItemTemplate);
					}
					else
					{
						AddBreadcrumbItem(link, LastItemTemplate);
					}
					Controls.AddAt(0, new LiteralControl(Environment.NewLine));
					++added;
				}
			}

			if (added < VisibilityLevel)
				Visible = false;

			base.CreateChildControls();
		}

		private void AddBreadcrumbItem(ILink link, ITemplate template)
		{
			BreadcrumbItem control = new BreadcrumbItem { DataItem = link };
			template.InstantiateIn(control);
			Controls.AddAt(0, control);
			control.DataBind();
		}
	}
}