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

		public Type TypeFilter { get; set; }

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			ChildrenEditor childrenEditor = (ChildrenEditor) editor;
			for (int i = 0; i < childrenEditor.ItemEditors.Count; i++)
			{
				if (childrenEditor.DeletedIndexes.Contains(i))
				{
					if (((ContentItem) childrenEditor.ItemEditors[i].CurrentItem).ID != 0)
						Context.Persister.Delete((ContentItem) childrenEditor.ItemEditors[i].CurrentItem);
				}
				else
				{
					ItemEditView childEditor = childrenEditor.ItemEditors[i];
					ItemEditView parentEditor = editor.Parent.FindParent<ItemEditView>();
					parentEditor.Saved += delegate { childEditor.Save(); };
				}
			}
			return childrenEditor.DeletedIndexes.Count > 0 || childrenEditor.AddedTypes.Count > 0;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			ChildrenEditor listEditor = (ChildrenEditor) editor;
			listEditor.ParentItem = (ContentItem) item;
		}

		protected override Control AddEditor(Control container)
		{
			ChildrenEditor childrenEditor = new ChildrenEditor
			{
				ID = Name,
				ParentItem = (ContentItem) container.FindParent<IEditableObjectEditor>().CurrentItem,
			};
			if (TypeFilter != null)
				childrenEditor.TypeFilter = TypeFilter.ToString();
			container.Controls.Add(childrenEditor);
			return childrenEditor;
		}

		protected override void DisableEditor(Control editor)
		{
			((ChildrenEditor) editor).Enabled = false;
		}
	}
}