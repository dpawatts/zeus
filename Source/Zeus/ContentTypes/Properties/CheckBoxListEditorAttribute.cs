using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Zeus.ContentTypes.Properties
{
	public abstract class CheckBoxListEditorAttribute : AbstractEditorAttribute
	{
		#region Constructors

		public CheckBoxListEditorAttribute()
		{
		}

		public CheckBoxListEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		#endregion

		public override bool UpdateItem(ContentItem item, Control editor)
		{
			CheckBoxList cbl = editor as CheckBoxList;
			IEnumerable selected = GetSelectedItems(cbl.Items.Cast<ListItem>().Where(li => li.Selected));
			DetailCollection dc = item.GetDetailCollection(this.Name, true);
			dc.Replace(selected);

			return true;
		}

		protected abstract IEnumerable GetSelectedItems(IEnumerable<ListItem> selectedListItems);

		public override void UpdateEditor(ContentItem item, Control editor)
		{
			CheckBoxList cbl = editor as CheckBoxList;
			cbl.Items.AddRange(GetListItems(item));

			DetailCollection dc = item.GetDetailCollection(this.Name, false);
			if (dc != null)
			{
				foreach (object detail in dc)
				{
					string value = GetValue(detail);
					ListItem li = cbl.Items.FindByValue(value);
					if (li != null)
					{
						li.Selected = true;
					}
					else
					{
						li = new ListItem(value);
						li.Selected = true;
						li.Attributes["style"] = "color:silver";
						cbl.Items.Add(li);
					}
				}
			}
		}

		protected abstract string GetValue(object detail);

		protected override Control AddEditor(Control container)
		{
			CheckBoxList cbl = new CheckBoxList();
			cbl.CssClass += " checkBoxList";
			cbl.RepeatLayout = RepeatLayout.Flow;
			container.Controls.Add(cbl);
			return cbl;
		}

		protected abstract ListItem[] GetListItems(ContentItem item);
	}
}
