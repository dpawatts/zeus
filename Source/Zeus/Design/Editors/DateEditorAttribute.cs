using System;
using System.Web.UI;
using Coolite.Ext.Web;
using Zeus.ContentTypes;

namespace Zeus.Design.Editors
{
	public class DateEditorAttribute : AbstractEditorAttribute
	{
		public DateEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		public DateEditorAttribute()
		{
			
		}

		protected override void DisableEditor(Control editor)
		{
			((DateField) editor).Enabled = false;
			((DateField) editor).ReadOnly = true;
		}

		protected override Control AddEditor(Control container)
		{
			DateField tb = new DateField();
			tb.ID = Name;
			if (Required)
			{
				tb.AllowBlank = false;
				tb.Cls = "required";
			}
			container.Controls.Add(tb);

			return tb;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			DateField tb = (DateField) editor;
			if (item[Name] != null)
				tb.SelectedDate = (DateTime) item[Name];
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			DateField tb = editor as DateField;
			if (!AreEqual(tb.SelectedDate, item[Name]))
			{
				item[Name] = tb.SelectedDate;
				return true;
			}
			return false;
		}
	}
}