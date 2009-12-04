using System.Collections.Generic;
using System.Web.UI;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	public abstract class BaseDetailCollectionEditorAttribute : AbstractEditorAttribute
	{
		protected BaseDetailCollectionEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			PropertyCollection detailCollection = item.GetDetailCollection(Name, true);
			BaseDetailCollectionEditor detailCollectionEditor = (BaseDetailCollectionEditor) editor;

			List<PropertyData> propertyDataToDelete = new List<PropertyData>();

			// First pass saves or creates items.
			for (int i = 0; i < detailCollectionEditor.Editors.Count; i++)
			{
				if (!detailCollectionEditor.DeletedIndexes.Contains(i))
				{
					PropertyData existingDetail = (detailCollection.Count > i) ? detailCollection.Details[i] : null;
					object newDetail;
					CreateOrUpdateDetailCollectionItem((ContentItem) item, existingDetail, detailCollectionEditor.Editors[i], out newDetail);
					if (newDetail != null)
						if (existingDetail != null)
							existingDetail.Value = newDetail;
						else
							detailCollection.Add(newDetail);
				}
				else
				{
					propertyDataToDelete.Add(detailCollection.Details[i]);
				}
			}

			// Do a second pass to delete the items, this is so we don't mess with the indices on the first pass.
			foreach (PropertyData propertyData in propertyDataToDelete)
				detailCollection.Remove(propertyData);

			return detailCollectionEditor.DeletedIndexes.Count > 0 || detailCollectionEditor.AddedEditors.Count > 0;
		}

		protected abstract void CreateOrUpdateDetailCollectionItem(ContentItem item,PropertyData existingDetail, Control editor, out object newDetail);
		protected abstract BaseDetailCollectionEditor CreateEditor();

		protected override Control AddEditor(Control container)
		{
			BaseDetailCollectionEditor detailCollectionEditor = CreateEditor();
			detailCollectionEditor.ID = Name;
			container.Controls.Add(detailCollectionEditor);
			return detailCollectionEditor;
		}

		protected override void DisableEditor(Control editor)
		{
			((BaseDetailCollectionEditor) editor).Enabled = false;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			BaseDetailCollectionEditor detailCollectionEditor = (BaseDetailCollectionEditor) editor;
			PropertyCollection detailCollection = item.GetDetailCollection(Name, true);
			PropertyData[] details = new PropertyData[detailCollection.Count];
			detailCollection.CopyTo(details, 0);
			detailCollectionEditor.Initialize(details);
		}
	}
}