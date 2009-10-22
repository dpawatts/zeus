using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Zeus.BaseLibrary.ExtensionMethods.Linq;
using Zeus.ContentTypes;

namespace Zeus.Design.Editors
{
	public class LinkedItemsCheckBoxListEditorAttribute : CheckBoxListEditorAttribute
	{
		public LinkedItemsCheckBoxListEditorAttribute()
		{
		}

		public LinkedItemsCheckBoxListEditorAttribute(string title, int sortOrder, Type typeFilter)
			: base(title, sortOrder)
		{
			TypeFilter = typeFilter;
		}

		public Type TypeFilter { get; set; }

		protected override IEnumerable GetSelectedItems(IEnumerable<ListItem> selectedListItems)
		{
			List<ContentItem> result = new List<ContentItem>();
			foreach (ListItem listItem in selectedListItems)
				result.Add(Context.Persister.Get(Convert.ToInt32(listItem.Value)));
			return result;
		}

		protected override string GetValue(object detail)
		{
			return ((ContentItem) detail).ID.ToString();
		}

		protected override ListItem[] GetListItems(IEditableObject item)
		{
			IQueryable<ContentItem> contentItems = Context.Current.Finder.QueryItems();
			var tempContentItems = ((IQueryable) contentItems).OfType(TypeFilter).OfType<object>();
			return tempContentItems.ToArray().Cast<ContentItem>()
				.Select(p => new ListItem(p.Title, p.ID.ToString()))
				.ToArray();
		}
	}
}