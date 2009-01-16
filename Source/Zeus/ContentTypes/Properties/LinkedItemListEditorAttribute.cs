using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using Zeus.Web.UI.WebControls;

namespace Zeus.ContentTypes.Properties
{
	public abstract class LinkedItemListEditorAttribute : AbstractEditorAttribute
	{
		#region Constructors

		protected LinkedItemListEditorAttribute()
		{
		}

		protected LinkedItemListEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		#endregion

		#region Properties

		public abstract IQueryable<ContentItem> GetItems();

		#endregion

		public override bool UpdateItem(ContentItem item, Control editor)
		{
			LinkedItemList cbl = editor as LinkedItemList;
			IEnumerable<ContentItem> selectedItems = cbl.SelectedContentItems;
			DetailCollection dc = item.GetDetailCollection(this.Name, true);
			dc.Replace(selectedItems);

			return true;
		}

		public override void UpdateEditor(ContentItem item, Control editor)
		{
			LinkedItemList cbl = editor as LinkedItemList;
			cbl.AvailableItems = GetItems().OrderBy(ci => ci.Title).Select(ci => new ListItem(ci.Title, ci.ID.ToString())).ToArray();

			DetailCollection dc = item.GetDetailCollection(this.Name, false);
			if (dc != null)
				cbl.SetSelectedItems(dc.Cast<ContentItem>().Select(ci => ci.ID.ToString()).ToArray());
		}

		protected override Control AddEditor(Control container)
		{
			LinkedItemList cbl = new LinkedItemList();
			container.Controls.Add(cbl);
			return cbl;
		}
	}
}
