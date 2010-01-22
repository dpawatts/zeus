using System;
using System.Web.UI;
using Ext.Net;
using Zeus.ContentTypes;

namespace Zeus.Design.Editors
{
	public class DateEditorAttribute : AbstractEditorAttribute
	{
		public bool IncludeTime { get; set; }

		public DateEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		public DateEditorAttribute()
		{
			
		}

		protected override void DisableEditor(Control editor)
		{
			MultiField placeHolder = (MultiField) editor;
			placeHolder.Fields[0].Enabled = false;
			((DateField) placeHolder.Fields[0]).ReadOnly = true;
			if (IncludeTime)
			{
				placeHolder.Fields[1].Enabled = false;
				((TimeField) placeHolder.Fields[1]).ReadOnly = true;
			}
		}

		protected override Control AddEditor(Control container)
		{
			MultiField placeHolder = new MultiField();

			DateField tb = new DateField();
			tb.ID = Name;
			if (Required)
			{
				tb.AllowBlank = false;
				tb.Cls = "required";
			}
			placeHolder.Fields.Add(tb);

			if (IncludeTime)
			{
				TimeField timeField = new TimeField();
				timeField.ID = Name + "Time";
				timeField.Width = 70;
				if (Required)
				{
					timeField.AllowBlank = false;
					timeField.Cls += " required";
				}
				placeHolder.Fields.Add(timeField);
			}

			container.Controls.Add(placeHolder);
			container.Controls.Add(new LiteralControl("<br />"));

			return placeHolder;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			MultiField placeHolder = (MultiField) editor;
			DateField tb = (DateField) placeHolder.Fields[0];
			if (item[Name] != null)
			{
				tb.SelectedDate = (DateTime) item[Name];
				if (IncludeTime)
					((TimeField) placeHolder.Fields[1]).SelectedTime = ((DateTime) item[Name]).TimeOfDay;
			}
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			MultiField placeHolder = (MultiField) editor;
			DateField tb = (DateField) placeHolder.Fields[0];
			bool result = false;
			DateTime? currentDate = item[Name] as DateTime?;
			if ((currentDate != null && tb.SelectedDate.Date != currentDate.Value) || currentDate == null)
			{
				currentDate = tb.SelectedDate.Date.Add((currentDate ?? DateTime.Now).TimeOfDay);
				item[Name] = currentDate;
				result = true;
			}
			if (IncludeTime)
			{
				TimeField timeField = (TimeField) placeHolder.Fields[1];
				if ((currentDate != null && timeField.SelectedTime != currentDate.Value.TimeOfDay) || currentDate == null)
				{
					DateTime newDate = (currentDate ?? DateTime.Now).Date.Add(timeField.SelectedTime);
					item[Name] = newDate;
					result = true;
				}
			}
			return result;
		}
	}
}