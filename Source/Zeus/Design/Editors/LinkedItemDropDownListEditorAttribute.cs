using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Linq;
using Zeus.ContentTypes;

namespace Zeus.Design.Editors
{
	public class LinkedItemDropDownListEditor : DropDownListEditorAttribute
	{
		#region Constructors

		public LinkedItemDropDownListEditor()
		{
		}

		public LinkedItemDropDownListEditor(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		#endregion

		public bool ExcludeSelf { get; set; }

		public Type TypeFilter { get; set; }

		protected override object GetValue(ListControl ddl)
		{
			if (!string.IsNullOrEmpty(ddl.SelectedValue))
				return Context.Current.Persister.Get(Convert.ToInt32(ddl.SelectedValue));
			return null;
		}

		protected override object GetValue(IEditableObject item)
		{
			ContentItem linkedItem = (ContentItem) item[Name];
			if (linkedItem != null)
				return linkedItem.ID.ToString();
			return string.Empty;
		}

		protected override ListItem[] GetListItems(IEditableObject item)
		{
			IEnumerable<ContentItem> items = Context.Current.Finder.Items().Distinct(ci => ci.ID);
			if (TypeFilter != null)
				items = items.OfType(TypeFilter);
			if (ExcludeSelf)
				items = items.Where(i => i != item);
			return items.OrderBy(i => i.HierarchicalTitle)
				.Select(i => new ListItem {Value = i.ID.ToString(), Text = i.HierarchicalTitle})
				.ToArray();
		}
	}
}