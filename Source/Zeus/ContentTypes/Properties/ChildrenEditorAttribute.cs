using System;
using System.Web.UI;
using Zeus.Web.UI;
using Zeus.Web.UI.WebControls;

namespace Zeus.ContentTypes.Properties
{
	public class ChildrenEditorAttribute : AbstractEditorAttribute
	{
		public ChildrenEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		public Type TypeFilter
		{
			get;
			set;
		}

		public override bool UpdateItem(ContentItem item, Control editor)
		{
			ItemEditorList listEditor = (ItemEditorList) editor;
			for (int i = 0; i < listEditor.ItemEditors.Count; i++)
			{
				if (listEditor.DeletedIndexes.Contains(i))
				{
					Context.Persister.Delete(listEditor.ItemEditors[i].CurrentItem);
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

		public override void UpdateEditor(ContentItem item, Control editor)
		{
			ItemEditorList listEditor = (ItemEditorList) editor;
			listEditor.ParentItem = item;
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
			listEditor.ParentItem = container.FindParent<IItemView>().CurrentItem;
			listEditor.TypeFilter = this.TypeFilter.ToString();
			container.Controls.Add(listEditor);
			return listEditor;
		}
	}
}
