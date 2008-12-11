using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.Linq;
using Zeus.Web.UI;

namespace Zeus.ContentTypes.Properties
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

		public bool ExcludeSelf
		{
			get;
			set;
		}

		public Type TypeFilter
		{
			get;
			set;
		}

		protected override object GetValue(DropDownList ddl)
		{
			if (!string.IsNullOrEmpty(ddl.SelectedValue))
				return Zeus.Context.Current.Persister.Get(Convert.ToInt32(ddl.SelectedValue));
			else
				return null;
		}

		protected override string GetValue(ContentItem item)
		{
			ContentItem linkedItem = (ContentItem) item[this.Name];
			if (linkedItem != null)
				return linkedItem.ID.ToString();
			else
				return string.Empty;
		}

		protected override ListItem[] GetListItems(ContentItem item)
		{
			IEnumerable<ContentItem> items = Zeus.Context.Current.Database.ContentItems;
			if (this.TypeFilter != null)
				items = items.OfType(this.TypeFilter);
			if (this.ExcludeSelf)
				items = items.Where(i => i != item);
			return items.OrderBy(i => i.Title)
				.Select(i => new ListItem { Value = i.ID.ToString(), Text = i.Title })
				.ToArray();
		}
	}
}
