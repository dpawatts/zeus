using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Coolite.Ext.Web;
using Zeus.ContentProperties;
using System.Web.UI;
using ImageButton = System.Web.UI.WebControls.ImageButton;
using Label = System.Web.UI.WebControls.Label;
using WebControl = System.Web.UI.WebControls.WebControl;

namespace Zeus.Web.UI.WebControls
{
	public abstract class BaseDetailCollectionEditor : WebControl, INamingContainer
	{
		#region Fields

		private PlaceHolder itemEditorsContainer;
		private readonly List<Control> _editors = new List<Control>();
		private IEnumerable<PropertyData> _initialValues = new List<PropertyData>();
		private int _editorIndex;

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

		public IList<string> AddedEditors
		{
			get
			{
				IList<string> result = ViewState["AddedEditors"] as IList<string>;
				if (result == null)
					ViewState["AddedEditors"] = result = new List<string>();
				return result;
			}
		}

		protected override HtmlTextWriterTag TagKey
		{
			get { return HtmlTextWriterTag.Div; }
		}

		public IList<Control> Editors
		{
			get { return _editors; }
		}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			UpdatePanel updatePanel = AddUpdatePanel();

			itemEditorsContainer = new PlaceHolder();
			updatePanel.ContentTemplateContainer.Controls.Add(itemEditorsContainer);
			AddNewItemDdl(updatePanel);

			Controls.Add(new LiteralControl("<br style=\"clear:both\" />"));
		}

		public void Initialize(IEnumerable<PropertyData> linkedItemDetails)
		{
			_initialValues = linkedItemDetails;
		}

		private UpdatePanel AddUpdatePanel()
		{
			UpdatePanel updatePanel = new UpdatePanel { ID = "updatePanel" };
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
			foreach (PropertyData linkDetail in _initialValues)
				CreateLinkedItemEditor(linkDetail);
			foreach (string id in AddedEditors)
				CreateLinkedItemEditor(null);

			base.CreateChildControls();
		}

		private void AddNewItemDdl(UpdatePanel container)
		{
			container.ContentTemplateContainer.Controls.Add(new Label { Text = "Add " + Title });

			ImageButton b = new ImageButton();
			container.ContentTemplateContainer.Controls.Add(b);
			b.ID = "addNew";
			b.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(ItemEditorList), "Zeus.Web.UI.WebControls.Images.add.png");
			b.ToolTip = "Add item";
			b.CausesValidation = false;
			b.Click += AddItemClick;
			b.CssClass = "add";
		}

		private static string GetNewID()
		{
			return Guid.NewGuid().ToString().Replace("-", string.Empty);
		}

		private void AddItemClick(object sender, ImageClickEventArgs e)
		{
			AddedEditors.Add(string.Empty);
			CreateLinkedItemEditor(null);
		}

		private void CreateLinkedItemEditor(PropertyData detail)
		{
			PlaceHolder container = new PlaceHolder { ID = "plc" + _editorIndex };
			AddDeleteButton(container, _editorIndex);
			Control editor = CreateDetailEditor(_editorIndex, detail);
			AddToContainer(container, editor);
			itemEditorsContainer.Controls.Add(container);
			Editors.Add(editor);
			++_editorIndex;
		}

		protected abstract Control CreateDetailEditor(int id, PropertyData detail);

		private void AddDeleteButton(Control container, int id)
		{
			ImageButton b = new ImageButton();
			container.Controls.Add(b);
			b.ID = ID + "_d_" + id;
			b.CssClass = " delete";
			b.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(BaseDetailCollectionEditor), "Zeus.Web.UI.WebControls.Images.delete.png");
			b.ToolTip = "Delete item";
			b.CommandArgument = id.ToString();
			b.Click += DeleteItemClick;
		}

		private void DeleteItemClick(object sender, ImageClickEventArgs e)
		{
			ImageButton b = (ImageButton) sender;
			b.Enabled = false;
			b.CssClass += " deleted";

			int index = int.Parse(b.CommandArgument);
			DeletedIndexes.Add(index);
			((FieldSet) Editors[index].Parent).CssClass = "deleted";
		}

		private void AddToContainer(Control container, Control itemEditor)
		{
			FieldSet fs = new FieldSet { Title = Title };
			container.Controls.Add(fs);
			fs.BodyControls.Add(itemEditor);
		}

		protected abstract string Title
		{
			get;
		}
	}
}
