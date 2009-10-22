using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.ContentProperties;
using Zeus.Web.UI.WebControls;
using DropDownList=System.Web.UI.WebControls.DropDownList;

namespace Zeus.Design.Editors
{
	public class LinkedItemsEditorAttribute : BaseDetailCollectionEditorAttribute
	{
		public LinkedItemsEditorAttribute(string title, int sortOrder, Type typeFilter)
			: base(title, sortOrder)
		{
			TypeFilter = typeFilter;
		}

		public Type TypeFilter { get; set; }

		protected override BaseDetailCollectionEditor CreateEditor()
		{
			return new LinkedItemsEditor { ID = Name, TypeFilter = TypeFilter.ToString() };
		}

		protected override void CreateOrUpdateDetailCollectionItem(ContentItem contentItem, PropertyData existingDetail, Control editor, out object newDetail)
		{
			DropDownList ddl = (DropDownList) editor;
			newDetail = Context.Persister.Get(Convert.ToInt32(ddl.SelectedValue));
		}
	}
}