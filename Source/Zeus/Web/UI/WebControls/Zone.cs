using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.Web.UI.WebControls;

namespace Zeus.Web.UI.WebControls
{
	/// <summary>
	/// Adds sub-items with a certain zone name to a page.
	/// </summary>
	[PersistChildren(false)]
	[ParseChildren(true)]
	public class Zone : ItemAwareControl
	{
		private bool isDataBound;
		private IEnumerable<ContentItem> items;

		/// <summary>Gets or sets the zone from which to fetch items.</summary>
		public string ZoneName
		{
			get { return (string) ViewState["ZoneName"] ?? string.Empty; }
			set { ViewState["ZoneName"] = value; }
		}

		/// <summary>Gets or sets a list of items to display.</summary>
		public virtual IEnumerable<ContentItem> DataSource
		{
			get
			{
				if (items == null)
					items = GetItems();
				return items;
			}
			set
			{
				items = value;
				isDataBound = false;
			}
		}

		/// <summary>Inserted between added child items.</summary>
		[DefaultValue((string) null), Browsable(false), PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(SimpleTemplateContainer))]
		public virtual ITemplate SeparatorTemplate { get; set; }

		/// <summary>Inserted before the zone control if a control was added.</summary>
		[DefaultValue((string) null), Browsable(false), PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(SimpleTemplateContainer))]
		public virtual ITemplate HeaderTemplate { get; set; }

		/// <summary>Added after the zone control if a control was added.</summary>
		[DefaultValue((string) null), Browsable(false), PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(SimpleTemplateContainer))]
		public virtual ITemplate FooterTemplate { get; set; }

		protected override void OnInit(EventArgs e)
		{
			EnsureChildControls();
			base.OnInit(e);
		}

		protected override void OnDataBinding(EventArgs e)
		{
			if (!isDataBound)
				EnsureChildControls();

			base.OnDataBinding(e);
		}

		private IEnumerable<ContentItem> GetItems()
		{
			IEnumerable<ContentItem> items = null;
			if (CurrentItem != null)
				items = CurrentPage.GetChildren(ZoneName);

			return items;
		}

		protected override void CreateChildControls()
		{
			CreateItems(this);
			base.CreateChildControls();
		}

		protected virtual void CreateItems(Control container)
		{
			if (DataSource != null)
			{
				container.Controls.Clear();
				bool firstPass = true;
				foreach (ContentItem item in DataSource)
				{
					if (firstPass)
					{
						firstPass = false;
						AppendTemplate(HeaderTemplate, container);
					}
					else if (SeparatorTemplate != null)
						AppendTemplate(SeparatorTemplate, container);

					AddChildItem(container, item);
				}
				if (!firstPass)
					AppendTemplate(FooterTemplate, container);

				isDataBound = true;
				ChildControlsCreated = true;
			}
		}

		private void AppendTemplate(ITemplate template, Control container)
		{
			if (template != null)
			{
				PlaceHolder ph = new PlaceHolder();
				container.Controls.Add(ph);
				template.InstantiateIn(ph);
			}
		}

		protected virtual void AddChildItem(Control container, ContentItem item)
		{
			ItemUtility.AddUserControl(container, item);
		}
	}
}
