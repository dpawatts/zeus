using System;
using System.Web.UI;
using Zeus.Web.UI.WebControls;

namespace Zeus.ContentTypes.Properties
{
	public class LinkedItemsEditorAttribute : AbstractEditorAttribute
	{
		public LinkedItemsEditorAttribute(string title, int sortOrder, Type typeFilter)
			: base(title, sortOrder)
		{
			TypeFilter = typeFilter;
		}

		public Type TypeFilter
		{
			get;
			set;
		}

		public override bool UpdateItem(ContentItem item, Control editor)
		{
			DetailCollection detailCollection = item.GetDetailCollection(Name, true);
			detailCollection.Clear();

			LinkedItemsEditor listEditor = (LinkedItemsEditor) editor;
			for (int i = 0; i < listEditor.DropDownLists.Count; i++)
				detailCollection.Add(Context.Persister.Get(Convert.ToInt32(listEditor.DropDownLists[i].SelectedValue)));
			return true;
		}

		public override void UpdateEditor(ContentItem item, Control editor)
		{
			LinkedItemsEditor listEditor = (LinkedItemsEditor) editor;
			DetailCollection detailCollection = item.GetDetailCollection(Name, true);
			LinkDetail[] linkDetails = new LinkDetail[detailCollection.Count];
			detailCollection.CopyTo(linkDetails, 0);
			listEditor.Initialize(linkDetails);
		}

		public override Control AddTo(Control container)
		{
			Control panel = AddPanel(container);
			return AddEditor(panel);
		}

		protected override Control AddEditor(Control container)
		{
			LinkedItemsEditor listEditor = new LinkedItemsEditor();
			listEditor.ID = Name;
			listEditor.TypeFilter = TypeFilter.ToString();
			container.Controls.Add(listEditor);
			return listEditor;
		}
	}
}
