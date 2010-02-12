using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Ext.Net;
using Zeus.ContentTypes;
using System.Web.UI;
using System.Web.Compilation;
using System.Linq;
using ImageButton = System.Web.UI.WebControls.ImageButton;
using Label = System.Web.UI.WebControls.Label;
using ListItem = System.Web.UI.WebControls.ListItem;
using WebControl = System.Web.UI.WebControls.WebControl;

namespace Zeus.Web.UI.WebControls
{
	public class ItemEditorList : WebControl, INamingContainer
	{
		#region Fields

		private System.Web.UI.WebControls.DropDownList types;
		private PlaceHolder itemEditorsContainer;
		private ContentItem parentItem;
		private List<string> addedTypes = new List<string>();
		private readonly List<ItemEditView> itemEditors = new List<ItemEditView>();
		private readonly List<ImageButton> _deleteButtons = new List<ImageButton>();
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

		/// <summary>Gets the text to use for the "add new" label.</summary>
		public string AddNewText
		{
			get { return ViewState["AddNewText"] as string; }
			set { ViewState["AddNewText"] = value; }
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

		public override bool Enabled
		{
			get { return base.Enabled; }
			set
			{
				EnsureChildControls();
				_deleteButtons.ForEach(b => b.Enabled = value);
				itemEditors.ForEach(ie => ie.Enabled = value);
				base.Enabled = value;
			}
		}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			UpdatePanel updatePanel = AddUpdatePanel();

			itemEditorsContainer = new PlaceHolder();
			updatePanel.ContentTemplateContainer.Controls.Add(itemEditorsContainer);

			types = new System.Web.UI.WebControls.DropDownList { ID = "ddlTypes" };
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
			if (TypeFilter != null)
			{
				Type type = BuildManager.GetType(TypeFilter, true);
				allowedChildren = allowedChildren.Where(ct => type.IsAssignableFrom(ct.ItemType));
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
				IList<ContentItem> items = ParentItem.GetChildren().ToList();
				if (TypeFilter != null)
				{
					Type type = BuildManager.GetType(TypeFilter, true);
					items = items.Where(ci => type.IsAssignableFrom(ci.GetType())).ToList();
				}
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
			if (item is WidgetContentItem)
				((WidgetContentItem) item).ZoneName = ZoneName;
			return item;
		}

		private void AddNewItemDdl(UpdatePanel container)
		{
			types = new System.Web.UI.WebControls.DropDownList { ID = "ddlTypes" };

			string labelText = AddNewText ?? "Add New Child";
			container.ContentTemplateContainer.Controls.Add(new Label { Text = labelText, AssociatedControlID = types.ID, CssClass = "editorLabel" });

			container.ContentTemplateContainer.Controls.Add(types);

			ImageButton b = new ImageButton();
			b.ID = "addNew";
			container.ContentTemplateContainer.Controls.Add(b);
			b.ImageUrl = Utility.GetCooliteIconUrl(Icon.Add);
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

		protected virtual void CreateItemEditor(ContentItem item)
		{
			AddDeleteButton();
			AddItemEditor(item);
			++itemEditorIndex;
		}

		private void AddDeleteButton()
		{
			ImageButton b = new ImageButton();
			itemEditorsContainer.Controls.Add(b);
			b.ID = ID + "_d_" + itemEditorIndex;
			b.CssClass = " delete";
			b.ImageUrl = Utility.GetCooliteIconUrl(Icon.Delete);
			b.ToolTip = "Delete item";
			b.CommandArgument = itemEditorIndex.ToString();
			b.CausesValidation = false;
			b.Click += DeleteItemClick;

			_deleteButtons.Add(b);
		}

		private void DeleteItemClick(object sender, ImageClickEventArgs e)
		{
			ImageButton b = (ImageButton) sender;
			b.Enabled = false;
			b.CssClass += " deleted";

			int index = int.Parse(b.CommandArgument);
			DeletedIndexes.Add(index);
			((FieldSet) ItemEditors[index].Parent).CssClass = "deleted";
			ItemEditors[index].Enabled = false;
			ItemEditors[index].CssClass += " deleted";
		}

		private void AddItemEditor(ContentItem item)
		{
			ItemEditView itemEditor = new ItemEditView();
			itemEditor.ID = ID + "_ie_" + itemEditorIndex;
			AddToContainer(itemEditorsContainer, itemEditor, item);
			itemEditors.Add(itemEditor);
			itemEditor.CurrentItem = item;
		}

		protected virtual void AddToContainer(Control container, ItemEditView itemEditor, ContentItem item)
		{
			FieldSet fs = new FieldSet();
			string status = (item.ID != 0) ? "ID #" + item.ID : "(Unsaved)";
            fs.Title = (item.ID != 0) ? Zeus.Context.Current.ContentTypes[item.GetType()].ContentTypeAttribute.Title + " " + status : "New Item " + status;
			container.Controls.Add(fs);
			fs.ContentControls.Add(itemEditor);
		}

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
	}
}
