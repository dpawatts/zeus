using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using Zeus.ContentTypes;

namespace Zeus.Web.UI.WebControls
{
	public class ChildrenEditor : WebControl
	{
		#region Fields

		private ContentItem _parentItem;
		private readonly List<ItemEditView> _itemEditors = new List<ItemEditView>();
		private int _itemEditorIndex = 0;
		private Store _store;
		private ChildrenEditorGridPanel _gridPanel;
		private List<string> _addedTypes = new List<string>();
		private List<int> _deletedIndexes = new List<int>();

		#endregion

		#region Properties

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

		public string TypeFilter
		{
			get { return (string) (ViewState["TypeFilter"] ?? null); }
			set { ViewState["TypeFilter"] = value; }
		}

		public ContentItem ParentItem
		{
			get
			{
				return _parentItem ?? (_parentItem = Zeus.Context.Persister.Get(ParentItemID));
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				_parentItem = value;
				ParentItemID = value.ID;
				ParentItemType = value.GetType().AssemblyQualifiedName;
			}
		}

		public ContentType CurrentItemDefinition
		{
			get { return Zeus.Context.ContentTypes[Type.GetType(ParentItemType)]; }
		}

		public List<ItemEditView> ItemEditors
		{
			get { return _itemEditors; }
		}

		public IList<string> AddedTypes
		{
			get { return _addedTypes; }
		}

		public IList<int> DeletedIndexes
		{
			get { return _deletedIndexes; }
		}

		#endregion

		protected override void LoadViewState(object savedState)
		{
			Triplet p = (Triplet) savedState;
			base.LoadViewState(p.First);
			_addedTypes = (List<string>) p.Second;
			_deletedIndexes = (List<int>) p.Third;
			EnsureChildControls();
		}

		protected override object SaveViewState()
		{
			return new Triplet(base.SaveViewState(), _addedTypes, _deletedIndexes);
		}

		protected override void OnInit(EventArgs e)
		{
			ExtNet.ResourceManager.RegisterIcon(Icon.Disk);
			ExtNet.ResourceManager.RegisterIcon(Icon.Cancel);
			base.OnInit(e);
		}

		protected override void CreateChildControls()
		{
			// Store

			JsonReader jsonReader = new JsonReader();
			jsonReader.IDProperty = "ID";
			jsonReader.Fields.Add("ID");
			jsonReader.Fields.Add("Title");
			jsonReader.Fields.Add("IconUrl");

			_store = new Store { ID = ID + "Store" };
			_store.Reader.Add(jsonReader);

			var items = GetItems();
			if (!ExtNet.IsAjaxRequest)
			{
				_store.DataSource = items;
				_store.DataBind();
			}

			Controls.Add(_store);

			// Grid Panel

			_gridPanel = new ChildrenEditorGridPanel { ID = ID + "GridPanel" };
			_gridPanel.AddItemButtonClicked += OnGridPanelAddItemButtonClicked;
			_gridPanel.ItemsRemoved += OnGridPanelItemsRemoved;
			Controls.Add(_gridPanel);

			_gridPanel.StoreID = _store.ID;

			// Editor Windows

			foreach (ContentItem contentItem in items)
				AddEditorWindow(contentItem);

			base.CreateChildControls();
		}

		private void OnGridPanelAddItemButtonClicked(object sender, ChildrenEditorGridPanelItemAddedEventArgs e)
		{
			_addedTypes.Add(e.ItemType.FullName + "," + e.ItemType.Assembly.FullName);

			ContentItem contentItem = CreateItem(e.ItemType);
			Window editorWindow = AddEditorWindow(contentItem);
			editorWindow.Render(this);

			ExtNet.ResourceManager.RegisterOnReadyScript(string.Format(@"
				var store = {0};
				var record = new store.recordType({{ IconUrl : '{1}' }}, 'New{3}');
				record.newRecord = true;
				store.add(record);
				
				var window = Ext.getCmp('{2}');
				window.record = record;
				window.show();",
											 _store.ClientID,
											 contentItem.IconUrl,
											 editorWindow.ClientID,
											 _addedTypes.Count));
		}

		private void OnGridPanelItemsRemoved(object sender, ChildrenEditorGridPanelItemsRemovedEventArgs e)
		{
			_deletedIndexes.AddRange(e.Indices);
		}

		private IList<ContentItem> GetItems()
		{
			if (ParentItem != null)
			{
				IList<ContentItem> items = ParentItem.GetChildren().ToList();
				if (TypeFilter != null)
				{
					Type type = BuildManager.GetType(TypeFilter, true);
					items = items.Where(ci => type.IsAssignableFrom(ci.GetType())).ToList();
				}
				foreach (string itemTypeName in _addedTypes)
				{
					ContentItem item = CreateItem(BuildManager.GetType(itemTypeName, true));
					items.Add(item);
				}
				return items;
			}
			return new ContentItem[0];
		}

		private ContentItem CreateItem(Type itemType)
		{
			ContentItem item = Zeus.Context.Current.ContentTypes.CreateInstance(itemType, ParentItem);
			/*if (item is WidgetContentItem)
				((WidgetContentItem) item).ZoneName = ZoneName;*/
			return item;
		}

		private Window AddEditorWindow(ContentItem contentItem)
		{
			Window editorWindow = new Window
			{
				ID = ID + "editorWindow" + _itemEditorIndex,
				Title = "Edit",
				Icon = Icon.NoteEdit,
				Width = 800,
				Height = 400,
				Modal = true,
				Hidden = true,
				Layout = "Fit",
				AutoScroll = true
			};

			Controls.Add(editorWindow);

			// Item Editor

			ItemEditView itemEditor = new ItemEditView();
			itemEditor.ID = ID + "_ie_" + _itemEditorIndex;
			editorWindow.ContentControls.Add(itemEditor);
			itemEditor.CurrentItem = contentItem;

			_itemEditors.Add(itemEditor);

			// Buttons

			Ext.Net.Button saveButton = new Ext.Net.Button
			{
				ID = ID + "btnSave" + _itemEditorIndex,
				Text = "Update",
				Icon = Icon.Disk
			};
			string titleValue = itemEditor.PropertyControls.ContainsKey("Title")
				? string.Format("Ext.getDom('{0}').value", itemEditor.PropertyControls["Title"].ClientID)
				: "'[No Title]'";
			saveButton.Listeners.Click.Handler = string.Format(@"
				var editorWindow = Ext.getCmp('{0}editorWindow{1}');
				var record = editorWindow.record;
				record.set('Title', {2});
				editorWindow.hide(null);",
				ClientID, _itemEditorIndex, titleValue);
			editorWindow.Buttons.Add(saveButton);

			++_itemEditorIndex;

			return editorWindow;
		}
	}
}