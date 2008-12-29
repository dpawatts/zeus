using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Zeus.ContentTypes;
using System.Web.UI;
using System.Web.Compilation;
using Zeus.Web.UI.HtmlControls;
using System.Linq;
using Isis.Linq;

[assembly: WebResource("Zeus.Web.UI.WebControls.Images.add.png", "image/png")]
[assembly: WebResource("Zeus.Web.UI.WebControls.Images.delete.png", "image/png")]
namespace Zeus.Web.UI.WebControls
{
	public class ItemEditorList : WebControl, INamingContainer
	{
		#region Fields

		private DropDownList types;
		private PlaceHolder itemEditorsContainer;
		private ContentItem parentItem;
		private List<string> addedTypes = new List<string>();
		private List<ItemEditView> itemEditors = new List<ItemEditView>();
		private List<int> deletedIndexes = new List<int>();
		private int itemEditorIndex = 0;

		#endregion

		#region Constructor

		public ItemEditorList()
		{
			CssClass = "itemEditorList";
		}

		#endregion

		#region Properties

		public string TypeFilter
		{
			get { return (string) (ViewState["TypeFilter"] ?? null); }
			set { ViewState["TypeFilter"] = value; }
		}

		protected override HtmlTextWriterTag TagKey
		{
			get { return HtmlTextWriterTag.Div; }
		}

		public List<ItemEditView> ItemEditors
		{
			get { return itemEditors; }
		}

		/// <summary>Gets the parent item where to look for items.</summary>
		public int ParentItemID
		{
			get { return (int) (ViewState["CurrentItemID"] ?? 0); }
			set { ViewState["CurrentItemID"] = value; }
		}

		/// <summary>Gets the parent item where to look for items.</summary>
		public string ParentItemType
		{
			get { return (string) (ViewState["CurrentItemType"] ?? string.Empty); }
			set { ViewState["CurrentItemType"] = value; }
		}

		/// <summary>Gets or sets the zone name to list.</summary>
		public string ZoneName
		{
			get { return (string) (ViewState["ZoneName"] ?? ""); }
			set { ViewState["ZoneName"] = value; }
		}

		public IList<string> AddedTypes
		{
			get { return addedTypes; }
		}

		public IList<int> DeletedIndexes
		{
			get { return deletedIndexes; }
		}

		public IContentTypeManager Definitions
		{
			get { return Zeus.Context.Current.ContentTypes; }
		}

		public ContentType CurrentItemDefinition
		{
			get { return Definitions[Type.GetType(ParentItemType)]; }
		}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			UpdatePanel updatePanel = AddUpdatePanel();

			itemEditorsContainer = new PlaceHolder();
			updatePanel.ContentTemplateContainer.Controls.Add(itemEditorsContainer);

			types = new DropDownList { ID = "ddlTypes" };
			AddNewItemDdl(updatePanel);
		}

		private UpdatePanel AddUpdatePanel()
		{
			UpdatePanel updatePanel = new UpdatePanel();
			updatePanel.ID = "updatePanel";
			this.Controls.Add(updatePanel);
			return updatePanel;
		}

		protected override void LoadViewState(object savedState)
		{
			Triplet p = (Triplet) savedState;
			base.LoadViewState(p.First);
			addedTypes = (List<string>) p.Second;
			deletedIndexes = (List<int>) p.Third;
			EnsureChildControls();

			//Console.WriteLine("addedTypes: " + addedTypes.Count + ", deletedIndexes: " + deletedIndexes.Count);
		}

		protected override void CreateChildControls()
		{
			foreach (ContentItem item in GetItems())
				CreateItemEditor(item);

			IEnumerable<ContentType> allowedChildren = CurrentItemDefinition.AllowedChildren;
			if (this.TypeFilter != null)
			{
				Type type = BuildManager.GetType(this.TypeFilter, true);
				allowedChildren = allowedChildren.Where(ct => ct.ItemType == type);
			}
			foreach (ContentType definition in allowedChildren)
			{
				ListItem li = new ListItem(definition.ContentTypeAttribute.Title, string.Format("{0},{1}", definition.ItemType.FullName, definition.ItemType.Assembly.FullName));
				types.Items.Add(li);
			}

			base.CreateChildControls();
		}

		protected override object SaveViewState()
		{
			return new Triplet(base.SaveViewState(), addedTypes, deletedIndexes);
		}

		public virtual IList<ContentItem> GetItems()
		{
			if (ParentItem != null)
			{
				IList<ContentItem> items = ParentItem.GetChildren();
				foreach (string itemTypeName in AddedTypes)
				{
					ContentItem item = CreateItem(BuildManager.GetType(itemTypeName, true));
					items.Add(item);
				}
				return items;
			}
			else
				return new ContentItem[0];
		}

		private ContentItem CreateItem(Type itemType)
		{
			ContentItem item = Zeus.Context.Current.ContentTypes.CreateInstance(itemType, ParentItem);
			item.ZoneName = ZoneName;
			return item;
		}

		private void AddNewItemDdl(UpdatePanel container)
		{
			types = new DropDownList { ID = "ddlTypes" };

			string labelText = (this.TypeFilter != null) ? "Add " + Zeus.Context.Current.ContentTypes[BuildManager.GetType(this.TypeFilter, true)].ContentTypeAttribute.Title : "Add New Child";
			container.ContentTemplateContainer.Controls.Add(new Label { Text = labelText, AssociatedControlID = types.ID, CssClass = "editorLabel" });

			container.ContentTemplateContainer.Controls.Add(types);

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
			AddedTypes.Add(types.SelectedValue);

			ContentItem item = CreateItem(BuildManager.GetType(types.SelectedValue, true));

			CreateItemEditor(item);
		}

		protected virtual IItemView CreateItemEditor(ContentItem item)
		{
			AddDeleteButton();
			IItemView itemEditor = AddItemEditor(item);
			++itemEditorIndex;
			return itemEditor;
		}

		private void AddDeleteButton()
		{
			ImageButton b = new ImageButton();
			itemEditorsContainer.Controls.Add(b);
			b.ID = ID + "_d_" + itemEditorIndex;
			b.CssClass = " delete";
			b.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(ItemEditorList), "Zeus.Web.UI.WebControls.Images.delete.png");
			b.ToolTip = "Delete item";
			b.CommandArgument = itemEditorIndex.ToString();
			b.Click += DeleteItemClick;
		}

		private void DeleteItemClick(object sender, ImageClickEventArgs e)
		{
			ImageButton b = (ImageButton) sender;
			b.Enabled = false;
			b.CssClass += " deleted";

			int index = int.Parse(b.CommandArgument);
			DeletedIndexes.Add(index);
			((HtmlFieldSet) ItemEditors[index].Parent).CssClass = "deleted";
			ItemEditors[index].Enabled = false;
			ItemEditors[index].CssClass += " deleted";
		}

		private IItemView AddItemEditor(ContentItem item)
		{
			ItemEditView itemEditor = new ItemEditView();
			itemEditor.ID = ID + "_ie_" + itemEditorIndex;
			AddToContainer(itemEditorsContainer, itemEditor, item);
			itemEditors.Add(itemEditor);
			itemEditor.CurrentItem = item;
			return itemEditor;
		}

		protected virtual void AddToContainer(Control container, ItemEditView itemEditor, ContentItem item)
		{
			HtmlFieldSet fs = new HtmlFieldSet();
			string status = (item.ID != 0) ? "ID #" + item.ID : "(Unsaved)";
			fs.Legend = Zeus.Context.Current.ContentTypes[item.GetType()].ContentTypeAttribute.Title + " " + status;
			container.Controls.Add(fs);
			fs.Controls.Add(itemEditor);
		}

		#region IItemContainer Members

		public ContentItem ParentItem
		{
			get
			{
				return parentItem ?? (parentItem = Zeus.Context.Persister.Get(ParentItemID));
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				parentItem = value;
				ParentItemID = value.ID;
				ParentItemType = value.GetType().AssemblyQualifiedName;
			}
		}

		#endregion
	}
}
