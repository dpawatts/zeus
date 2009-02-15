using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Zeus.ContentTypes;
using System.Web.UI;
using System.Web.Compilation;
using Zeus.ContentTypes.Properties;
using Zeus.Web.UI.HtmlControls;
using System.Linq;
using Isis.Linq;

namespace Zeus.Web.UI.WebControls
{
	public sealed class LinkedItemsEditor : WebControl, INamingContainer
	{
		#region Fields

		private PlaceHolder itemEditorsContainer;
		private readonly List<DropDownList> _dropDownLists = new List<DropDownList>();
		private IEnumerable<LinkDetail> _initialValues = new List<LinkDetail>();

		#endregion

		#region Constructor

		public LinkedItemsEditor()
		{
			CssClass = "linkedItemsEditor";
		}

		#endregion

		#region Properties

		public string TypeFilter
		{
			get { return (string) (ViewState["TypeFilter"] ?? null); }
			set { ViewState["TypeFilter"] = value; }
		}

		public bool AlreadyInitialized
		{
			get { return (bool) (ViewState["AlreadyInitialized"] ?? false); }
			set { ViewState["AlreadyInitialized"] = value; }
		}

		public List<string> CreatedDropDownListIDs
		{
			get
			{
				List<string> result = ViewState["CreatedDropDownListIDs"] as List<string>;
				if (result == null)
					ViewState["CreatedDropDownListIDs"] = result = new List<string>();
				return result;
			}
		}

		private Type TypeFilterInternal
		{
			get { return BuildManager.GetType(TypeFilter, true); }
		}

		/*private ContentType ContentType
		{
			get { return Zeus.Context.Current.ContentTypes[TypeFilterInternal]; }
		}*/

		protected override HtmlTextWriterTag TagKey
		{
			get { return HtmlTextWriterTag.Div; }
		}

		public List<DropDownList> DropDownLists
		{
			get { return _dropDownLists; }
		}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			UpdatePanel updatePanel = AddUpdatePanel();

			itemEditorsContainer = new PlaceHolder { ID = "itemEditorsContainer" };
			updatePanel.ContentTemplateContainer.Controls.Add(itemEditorsContainer);
			AddNewItemDdl(updatePanel);

			Controls.Add(new LiteralControl("<br style=\"clear:both\" />"));
		}

		public void Initialize(IEnumerable<LinkDetail> linkedItemDetails)
		{
			_initialValues = linkedItemDetails;
		}

		private UpdatePanel AddUpdatePanel()
		{
			UpdatePanel updatePanel = new UpdatePanel();
			updatePanel.ID = "updatePanel";
			Controls.Add(updatePanel);
			return updatePanel;
		}

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState(savedState);
			EnsureChildControls();
		}

		protected override void CreateChildControls()
		{
			if (!AlreadyInitialized)
			{
				foreach (LinkDetail linkDetail in _initialValues)
				{
					string id = Guid.NewGuid().ToString();
					CreateLinkedItemEditor(id, linkDetail.LinkValue.Value);
					CreatedDropDownListIDs.Add(id);
				}
				AlreadyInitialized = true;
			}
			else
			{
				foreach (string id in CreatedDropDownListIDs)
					CreateLinkedItemEditor(id, null);
			}

			base.CreateChildControls();
		}

		private void AddNewItemDdl(UpdatePanel container)
		{
			container.ContentTemplateContainer.Controls.Add(new Label { Text = "Add Product Recommendation" });

			ImageButton b = new ImageButton();
			b.ID = "addNew";
			container.ContentTemplateContainer.Controls.Add(b);
			b.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(ItemEditorList), "Zeus.Web.UI.WebControls.Images.add.png");
			b.ToolTip = "Add item";
			b.CausesValidation = false;
			b.Click += AddItemClick;
			b.CssClass = "add";
		}

		private void AddItemClick(object sender, ImageClickEventArgs e)
		{
			string id = Guid.NewGuid().ToString();
			CreatedDropDownListIDs.Add(id);
			CreateLinkedItemEditor(id, null);
		}

		private void CreateLinkedItemEditor(string id, int? selectedValue)
		{
			PlaceHolder container = new PlaceHolder { ID = "plc" + id };
			AddDeleteButton(container, id);
			AddItemEditor(container, id, selectedValue);
			itemEditorsContainer.Controls.Add(container);
		}

		private void AddDeleteButton(Control container, string id)
		{
			ImageButton b = new ImageButton();
			container.Controls.Add(b);
			b.ID = ID + "_d_" + id;
			b.CssClass = " delete";
			b.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(ItemEditorList), "Zeus.Web.UI.WebControls.Images.delete.png");
			b.ToolTip = "Delete item";
			b.CommandArgument = id;
			b.Click += DeleteItemClick;
		}

		private void DeleteItemClick(object sender, ImageClickEventArgs e)
		{
			ImageButton b = (ImageButton) sender;
			b.Enabled = false;
			b.CssClass += " deleted";

			CreatedDropDownListIDs.Remove(b.CommandArgument);
			DropDownList dropDownList = DropDownLists.Single(ddl => ddl.ID == ID + "_ddl_" + b.CommandArgument);
			DropDownLists.Remove(dropDownList);
			dropDownList.Parent.Parent.Controls.Remove(dropDownList.Parent);
		}

		private void AddItemEditor(Control container, string id, int? selectedValue)
		{
			DropDownList ddl = new DropDownList();
			ddl.CssClass = "linkedItem";
			ddl.ID = ID + "_ddl_" + id;
			IEnumerable first = Zeus.Context.Current.Finder.ToArray().AsQueryable().OfType(TypeFilterInternal);
			IEnumerable<ContentItem> contentItems = first.Cast<ContentItem>().OrderBy(ci => ci.Title);
			ddl.Items.AddRange(contentItems.Select(ci => new ListItem(ci.Title, ci.ID.ToString())).ToArray());
			if (selectedValue != null)
				ddl.SelectedValue = selectedValue.Value.ToString();
			AddToContainer(container, ddl, id);
			DropDownLists.Add(ddl);
			//ddl.CurrentItem = item;
		}

		private void AddToContainer(Control container, Control itemEditor, string id)
		{
			HtmlFieldSet fs = new HtmlFieldSet { ID = ID + "_fs_" + id, Legend = "Product Recommendation" };
			container.Controls.Add(fs);
			fs.Controls.Add(itemEditor);
		}
	}
}