using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	/// <summary>
	/// Decorates the content item with a time editor.
	/// </summary>
	public class TimeEditorAttribute : AbstractEditorAttribute
	{
		public TimeEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override Control AddEditor(Control container)
		{
			TimePicker range = new TimePicker { ID = Name };
			container.Controls.Add(range);
			return range;
		}

		protected override void DisableEditor(Control editor)
		{
			((TimePicker) editor).Enabled = false;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			TimePicker range = (TimePicker) editor;
			range.Text = (string) item[Name];
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			TimePicker range = (TimePicker) editor;
			if ((string) item[Name] != range.Text)
			{
				item[Name] = range.Text;
				return true;
			}
			return false;
		}
	}
}