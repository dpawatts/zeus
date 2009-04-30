using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.ContentProperties;
using Zeus.ContentTypes;

namespace Zeus.Design.Editors
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

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			CheckBoxList cbl = (CheckBoxList) editor;
			IEnumerable selected = GetSelectedItems(cbl.Items.Cast<ListItem>().Where(li => li.Selected));
			PropertyCollection dc = item.GetDetailCollection(Name, true);
			dc.Replace(selected);
			return true;
		}

		protected abstract IEnumerable GetSelectedItems(IEnumerable<ListItem> selectedListItems);

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			CheckBoxList cbl = (CheckBoxList) editor;
			cbl.Items.AddRange(GetListItems(item));

			PropertyCollection dc = item.GetDetailCollection(Name, false);
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
			CheckBoxList cbl = CreateEditor();
			cbl.CssClass += " checkBoxList";
			cbl.RepeatLayout = RepeatLayout.Flow;
			container.Controls.Add(cbl);

			container.Controls.Add(new LiteralControl("<br style=\"clear:both\" />"));

			return cbl;
		}

		protected override void DisableEditor(Control editor)
		{
			((CheckBoxList) editor).Enabled = false;
		}

		protected virtual CheckBoxList CreateEditor()
		{
			return new CheckBoxList();
		}

		protected abstract ListItem[] GetListItems(IEditableObject item);
	}
}