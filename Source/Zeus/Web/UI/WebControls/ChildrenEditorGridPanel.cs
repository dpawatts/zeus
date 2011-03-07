using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Compilation;
using Ext.Net;
using Zeus.ContentTypes;

namespace Zeus.Web.UI.WebControls
{
	public class ChildrenEditorGridPanel : GridPanel
	{
		private Button _removeItemsButton;

		public event EventHandler<ChildrenEditorGridPanelItemAddedEventArgs> AddItemButtonClicked;
		public event EventHandler<ChildrenEditorGridPanelItemsRemovedEventArgs> ItemsRemoved;

		private void OnInvokeAddItemButton(ChildrenEditorGridPanelItemAddedEventArgs e)
		{
			if (AddItemButtonClicked != null)
				AddItemButtonClicked(this, e);
		}

		private void OnItemsRemoved(ChildrenEditorGridPanelItemsRemovedEventArgs args)
		{
			if (ItemsRemoved != null)
				ItemsRemoved(this, args);
		}

		protected override void CreateChildControls()
		{
			Width = 500;
			Height = 300;
			StyleSpec = "float:left;margin-bottom:15px";

			CreateToolbar();
			CreateColumnModel();

			GridView gridView = new GridView();
			gridView.GetRowClass.Fn = "childrenEditorGetRowClass";
			View.Add(gridView);

			ResourceManager.RegisterClientStyleBlock("ChildrenEditorGridPanelStyles", @"
				.childrenEditor-deleted-row {
	        background: #F0AAB2;
        }
        
        .childrenEditor-new-row {
	        background: #c8ffc8;
        }");

			ResourceManager.RegisterClientScriptBlock("ChildrenEditorGridPanelScript", @"
				var childrenEditorGetRowClass = function (record) {
					if (record.newRecord) {
						return 'childrenEditor-new-row';
					}

					if (record.removed) {
						return 'childrenEditor-deleted-row';
					}
				}

				var childrenEditorRenderIcon = function(value, p, record) {
					return String.format('<img src=""{0}"" alt=""{1}"" />',
						value, record.data.Title);
				}

				var flagRemovedItems = function(gridPanel) {
					var selections = gridPanel.getSelectionModel().getSelections();
					for (var i = 0; i < selections.length; i++)
					{{
						selections[i].newRecord = false;
						selections[i].removed = true;
					}}
					gridPanel.getSelectionModel().clearSelections();
					gridPanel.getView().refresh();
				}

				var flagRemovedItem = function(gridPanel, index) {
					var record = gridPanel.getStore().getAt(index);
					record.newRecord = false;
					record.removed = true;
					gridPanel.getView().refresh();
				}

				var childrenEditorPrepareCommandToolbar = function (grid, toolbar, rowIndex, record) {
					// Could return false to hide toolbar.
					var button1 = toolbar.items.get(0);
					var button2 = toolbar.items.get(1);

					if (record.removed) {
						button1.setDisabled(true); button1.setTooltip('Disabled');
						button2.setDisabled(true); button2.setTooltip('Disabled');
					}
        };");

			base.CreateChildControls();
		}

		private IEnumerable<ContentType> GetAllowedChildren()
		{
			ChildrenEditor parent = (ChildrenEditor) Parent;
			IEnumerable<ContentType> allowedChildren = parent.CurrentItemDefinition.AllowedChildren;
			if (parent.TypeFilter != null)
			{
				Type type = BuildManager.GetType(parent.TypeFilter, true);
				allowedChildren = allowedChildren.Where(ct => type.IsAssignableFrom(ct.ItemType));
			}
			return allowedChildren;
		}

		private void CreateToolbar()
		{
			Toolbar topToolbar = new Toolbar();

			Button addItemButton = new Button("Add Item")
			{
				ID = ID + "btnAdd",
				Icon = Icon.Add
			};

			Menu addItemButtonMenu = new Menu();
			IEnumerable<ContentType> allowedChildren = GetAllowedChildren();
			foreach (ContentType definition in allowedChildren)
			{
				MenuItem menuItem = new MenuItem(definition.ContentTypeAttribute.Title)
				{
					IconUrl = definition.IconUrl
				};
				menuItem.DirectEvents.Click.ExtraParams["Type"] = string.Format("{0},{1}", definition.ItemType.FullName,
					definition.ItemType.Assembly.FullName);
				menuItem.DirectEvents.Click.Event += OnAddItemClick;
				addItemButtonMenu.Items.Add(menuItem);
			}
			addItemButton.Menu.Add(addItemButtonMenu);

			topToolbar.Items.Add(addItemButton);

			_removeItemsButton = new Button("Remove Item(s)")
			{
				ID = ID + "btnRemoveItems",
				Icon = Icon.Delete,
				Disabled = true
			};
			_removeItemsButton.DirectEvents.Click.ExtraParams.Add(new Parameter("DeletedItems", string.Format(@"
				(function() {{
					var gridPanel = {0};
					var selections = gridPanel.getSelectionModel().getSelections();
					var selectedIndexes = [];
					for (var i = 0; i < selections.length; i++)
						selectedIndexes.push(gridPanel.getStore().indexOf(selections[i]));
					return selectedIndexes.join(',');
				}}).call(this)", ClientID), ParameterMode.Raw));
			_removeItemsButton.DirectEvents.Click.Event += OnRemoveItemsClick;
			_removeItemsButton.DirectEvents.Click.Success = string.Format(@"flagRemovedItems({0});", ClientID);
			topToolbar.Items.Add(_removeItemsButton);
			TopBar.Add(topToolbar);
		}

		private void OnAddItemClick(object sender, DirectEventArgs e)
		{
			Type type = BuildManager.GetType(e.ExtraParams["Type"], true);
			OnInvokeAddItemButton(new ChildrenEditorGridPanelItemAddedEventArgs(type));
		}

		private void OnRemoveItemsClick(object sender, DirectEventArgs e)
		{
			string[] deletedItemsArray = e.ExtraParams["DeletedItems"].Split(',');
			int[] deletedItems = deletedItemsArray.Select(s => Convert.ToInt32(s)).ToArray();
			OnItemsRemoved(new ChildrenEditorGridPanelItemsRemovedEventArgs(deletedItems));
		}

		private void CreateColumnModel()
		{
			ColumnModel.Columns.Add(new RowNumbererColumn());

			Column iconColumn = new Column
			{
				ColumnID = "IconUrl",
				DataIndex = "IconUrl",
				Width = 30
			};
			iconColumn.Renderer.Fn = "childrenEditorRenderIcon";
			ColumnModel.Columns.Add(iconColumn);

			ColumnModel.Columns.Add(new Column
			{
				ColumnID = "ID",
				Header = "ID",
				DataIndex = "ID",
				Width = 50
			});
			ColumnModel.Columns.Add(new Column
			{
				ColumnID = "Title",
				Header = "Title",
				DataIndex = "Title",
				Width = 200
			});

			CommandColumn commandColumn = new CommandColumn
			{
				Width = 120,
				PrepareToolbar = { Fn = "childrenEditorPrepareCommandToolbar" }
			};
			AddCommand(commandColumn, Icon.NoteEdit, "Edit", "Edit");
			AddCommand(commandColumn, Icon.Delete, "Delete", "Remove");
			ColumnModel.Columns.Add(commandColumn);

			Listeners.Command.Handler = string.Format(@"
				switch (command)
				{{
					case 'Edit' :
						var editorWindow = Ext.getCmp('{0}editorWindow' + rowIndex);
						editorWindow.record = record;
						editorWindow.show();
						break;
				}}", Parent.ClientID);

			Listeners.RowClick.Handler = string.Format(@"
        var record = {0}.getStore().getAt(rowIndex);
        if (record.removed) {{
					{0}.getSelectionModel().deselectRow(rowIndex);
        }}", ClientID);

			DirectEvents.Command.Event += OnGridPanelCommand;
			DirectEvents.Command.ExtraParams.Add(new Parameter("Command", "command", ParameterMode.Raw));
			DirectEvents.Command.ExtraParams.Add(new Parameter("Index", "rowIndex", ParameterMode.Raw));
			DirectEvents.Command.Success = string.Format("if (extraParams['Command'] == 'Delete') flagRemovedItem({0}, extraParams['Index']);",
				ClientID);

			SelectionModel.Add(new RowSelectionModel
			{
				Listeners =
				{
					RowSelect = { Handler = _removeItemsButton.ClientID + ".enable();" },
					RowDeselect =
					{
						Handler = string.Format("if (!{0}.hasSelection()) {{ {1}.disable(); }}",
							this.ClientID, _removeItemsButton.ClientID)
					}
				}
			});
		}

		private static void AddCommand(CommandColumn commandColumn, Icon icon, string command, string text)
		{
			GridCommand gridCommand = new GridCommand
			{
				Icon = icon,
				Text = text,
				CommandName = command
			};
			commandColumn.Commands.Add(gridCommand);
		}

		private void OnGridPanelCommand(object sender, DirectEventArgs e)
		{
			switch (e.ExtraParams["Command"])
			{
				case "Delete" :
					OnItemsRemoved(new ChildrenEditorGridPanelItemsRemovedEventArgs(Convert.ToInt32(e.ExtraParams["Index"])));
					break;
			}
		}
	}

	public class ChildrenEditorGridPanelItemAddedEventArgs : EventArgs
	{
		public Type ItemType { get; set; }

		public ChildrenEditorGridPanelItemAddedEventArgs(Type itemType)
		{
			ItemType = itemType;
		}
	}

	public class ChildrenEditorGridPanelItemsRemovedEventArgs : EventArgs
	{
		public int[] Indices { get; set; }

		public ChildrenEditorGridPanelItemsRemovedEventArgs(params int[] indices)
		{
			Indices = indices;
		}
	}
}