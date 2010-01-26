using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Ext.Net;
using Ext.Net.Utilities;
using Zeus.ContentProperties;
using System.Web.UI;
using Button = Ext.Net.Button;
using Panel = Ext.Net.Panel;
using Parameter = Ext.Net.Parameter;

namespace Zeus.Web.UI.WebControls
{
	[DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.ClientID)]
	public abstract class BaseDetailCollectionEditor : WebControl
	{
		private class CustomContainer : Container
		{
			protected override System.Web.UI.HtmlControls.HtmlGenericControl CreateContainer()
			{
				System.Web.UI.HtmlControls.HtmlGenericControl control = base.CreateContainer();
				control.ID = ID.ConcatWith("_Content");
				return control;
			}
		}
		#region Fields

		private readonly List<Panel> _panels = new List<Panel>();
		private readonly List<Control> _editors = new List<Control>();
		private IEnumerable<PropertyData> _initialValues = new List<PropertyData>();
		private int _editorIndex;
		private Hidden _hiddenAddedEditors;
		private Container _container;

		#endregion

		#region Constructor

		protected BaseDetailCollectionEditor()
		{
			CssClass = "linkedItemsEditor";
		}

		#endregion

		#region Properties

		public IList<int> DeletedIndexes
		{
			get
			{
				IList<int> result = ViewState["DeletedIndexes"] as IList<int>;
				if (result == null)
					ViewState["DeletedIndexes"] = result = new List<int>();
				return result;
			}
		}

		public bool AlreadyInitialized
		{
			get { return (bool) (ViewState["AlreadyInitialized"] ?? false); }
			set { ViewState["AlreadyInitialized"] = value; }
		}

		protected override HtmlTextWriterTag TagKey
		{
			get { return HtmlTextWriterTag.Div; }
		}

		public IList<Panel> Panels
		{
			get { return _panels; }
		}

		public IList<Control> Editors
		{
			get { return _editors; }
		}

		public int AddedEditorCount
		{
			get
			{
				EnsureChildControls();
				return Convert.ToInt32(_hiddenAddedEditors.Text);
			}
			private set
			{
				EnsureChildControls();
				_hiddenAddedEditors.Text = value.ToString();
			}
		}

		public bool AddedEditors
		{
			get { return AddedEditorCount > 0; }
		}

		#endregion

		public void Initialize(IEnumerable<PropertyData> linkedItemDetails)
		{
			_initialValues = linkedItemDetails;
		}

		protected override void OnInit(EventArgs e)
		{
			_hiddenAddedEditors = new Hidden { ID = ID + "_hiddenAddedEditors", Text = @"0" };
			Controls.Add(_hiddenAddedEditors);

			base.OnInit(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			_container = new CustomContainer { ID = ID + "_container" };
			Controls.Add(_container);

			foreach (PropertyData linkDetail in _initialValues)
				CreateLinkedItemEditor(linkDetail);
			for (int i = 0; i < AddedEditorCount; ++i)
				CreateLinkedItemEditor(null);

			AddNewItemButton();
			Controls.Add(new LiteralControl("<br style=\"clear:both\" />") { ID = ID + "_literalControl" });

			base.OnLoad(e);
		}

		private void AddNewItemButton()
		{
			Controls.Add(new LiteralControl("<br style=\"clear:both\" />") { ID = ID + "_literalControl2" });

			var addButton = new Button
			{
				ID = ID + "_addButton",
				Icon = Icon.Add,
				Text = @"Add " + ItemTitle,
				CausesValidation = false
			};
			addButton.DirectClick += OnAddButtonDirectClick;
			Controls.Add(addButton);
		}

		private void OnAddButtonDirectClick(object sender, DirectEventArgs e)
		{
			++AddedEditorCount;
			Panel panel = CreateLinkedItemEditor(null);
			panel.Render(_container);
		}

		private Panel CreateLinkedItemEditor(PropertyData detail)
		{
			Control editor = CreateDetailEditor(_editorIndex, detail);
			Panel panel = CreatePanel(editor, _editorIndex);
			_container.Controls.Add(panel);
			Panels.Add(panel);
			Editors.Add(editor);
			++_editorIndex;

			return panel;
		}

		protected abstract Control CreateDetailEditor(int id, PropertyData detail);

		private Panel CreatePanel(Control itemEditor, int id)
		{
			var panel = new Panel
			{
				ID = ID + "_panel_" + id,
				IDMode = IDMode.Legacy,
				Header = false,
				BodyStyle = "padding:5px",
				StyleSpec = "margin-bottom:10px;"
			};
			var toolbar = new Toolbar();
			panel.TopBar.Add(toolbar);

			//toolbar.Items.Add(new Button { Icon = Icon.ArrowNsew });
			toolbar.Items.Add(new ToolbarFill());

			var deleteButton = new Button
			{
				ID = ID + "_deleteButton_" + id,
				IDMode = IDMode.Legacy,
				Icon = Icon.Delete,
				Text = @"Delete " + ItemTitle,
				CausesValidation = false
			};
			deleteButton.DirectEvents.Click.Event += OnDeleteButtonDirectClick;
			deleteButton.DirectEvents.Click.ExtraParams.Add(new Parameter("ID", id.ToString()));
			toolbar.Items.Add(deleteButton);

			panel.ContentControls.Add(itemEditor);

			return panel;
		}

		private void OnDeleteButtonDirectClick(object sender, DirectEventArgs e)
		{
			int index = int.Parse(e.ExtraParams["ID"]);
			DeletedIndexes.Add(index);
			Panels[index].Hidden = true;
			Panels[index].Update();
		}

		protected abstract string ItemTitle
		{
			get;
		}
	}
}
