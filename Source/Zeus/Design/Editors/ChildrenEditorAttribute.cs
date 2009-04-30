using System;
using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.Web.UI;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	public class ChildrenEditorAttribute : AbstractEditorAttribute
	{
		public ChildrenEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		public string AddNewText { get; set; }
		public Type TypeFilter { get; set; }

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			ItemEditorList listEditor = (ItemEditorList) editor;
			for (int i = 0; i < listEditor.ItemEditors.Count; i++)
			{
				if (listEditor.DeletedIndexes.Contains(i))
				{
					Context.Persister.Delete((ContentItem) listEditor.ItemEditors[i].CurrentItem);
				}
				else
				{
					ItemEditView childEditor = listEditor.ItemEditors[i];
					ItemEditView parentEditor = editor.Parent.FindParent<ItemEditView>();
					parentEditor.Saved += delegate { childEditor.Save(); };
				}
			}
			return listEditor.DeletedIndexes.Count > 0 || listEditor.AddedTypes.Count > 0;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			ItemEditorList listEditor = (ItemEditorList) editor;
			listEditor.ParentItem = (ContentItem) item;
		}

		public override Control AddTo(Control container)
		{
			Control panel = AddPanel(container);
			return AddEditor(panel);
		}

		protected override Control AddEditor(Control container)
		{
			ItemEditorList listEditor = new ItemEditorList();
			listEditor.ID = Name;
			listEditor.ParentItem = (ContentItem) container.FindParent<IEditableObjectEditor>().CurrentItem;
			if (TypeFilter != null)
				listEditor.TypeFilter = TypeFilter.ToString();
			listEditor.AddNewText = AddNewText;
			container.Controls.Add(listEditor);
			return listEditor;
		}

		protected override void DisableEditor(Control editor)
		{
			((ItemEditorList) editor).Enabled = false;
		}
	}
}