using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	/// <summary>
	/// Decorates the content item with a time range editable that will update two time fields.
	/// </summary>
	public class TimeRangeEditorAttribute : AbstractEditorAttribute
	{
		public TimeRangeEditorAttribute(string title, int sortOrder, string name, string nameEndRange)
			: base(title, sortOrder)
		{
			BetweenText = " - ";
			Name = name;
			NameEndRange = nameEndRange;
		}

		/// <summary>End of range detail (property) on the content item's object</summary>
		public string NameEndRange { get; set; }

		/// <summary>Gets or sets a text displayed between the date fields.</summary>
		public string BetweenText { get; set; }

		public string StartTitle { get; set; }
		public bool StartRequired { get; set; }

		protected override Control AddEditor(Control container)
		{
			TimeRange range = new TimeRange {ID = Name + NameEndRange, StartTitle = StartTitle, StartRequired = StartRequired};
			container.Controls.Add(range);
			range.BetweenText = BetweenText;
			return range;
		}

		protected override void DisableEditor(Control editor)
		{
			((TimeRange) editor).Enabled = false;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			TimeRange range = (TimeRange) editor;
			range.From = (string) item[Name];
			range.To = (string) item[NameEndRange];
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			TimeRange range = editor as TimeRange;
			if ((string) item[Name] != range.From || (string) item[NameEndRange] != range.To)
			{
				item[Name] = range.From;
				item[NameEndRange] = range.To;
				return true;
			}
			return false;
		}
	}
}