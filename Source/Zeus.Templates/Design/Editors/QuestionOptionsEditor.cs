using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Templates.ContentTypes.Forms;

namespace Zeus.Templates.Design.Editors
{
	public class QuestionOptionsEditorAttribute : AbstractEditorAttribute
	{
		public QuestionOptionsEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{

		}

		public override bool UpdateItem(IEditableObject itemTemp, Control editor)
		{
			ContentItem item = (ContentItem)itemTemp;
			TextBox tb = (TextBox)editor;
			string[] rows = tb.Text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			for (int i = item.Children.Count - 1; i >= 0; --i)
			{
				int index = Array.FindIndex(rows, row => row == item.Children[i].Title);
				if (index < 0)
					Context.Persister.Delete(item.Children[i]);
			}
			for (int i = 0; i < rows.Length; i++)
			{
				ContentItem child = FindChild(item, rows[i]);
				if (child == null)
				{
					child = new Option { Title = rows[i] };
					child.AddTo(item);
				}
				child.SortOrder = i;
			}

			return true;
		}

		private static ContentItem FindChild(ContentItem item, string row)
		{
			foreach (ContentItem child in item.Children)
				if (child.Title == row)
					return child;
			return null;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			TextBox tb = (TextBox)editor;
			tb.Text = string.Empty;
			foreach (ContentItem child in ((ContentItem)item).GetChildren())
				tb.Text += child.Title + Environment.NewLine;
		}

		protected override Control AddEditor(Control container)
		{
			TextBox tb = new TextBox { ID = Name, TextMode = TextBoxMode.MultiLine, Rows = 5, Columns = 40 };
			container.Controls.Add(tb);
			return tb;
		}

		protected override void DisableEditor(Control editor)
		{
			((TextBox) editor).Enabled = false;
		}
	}
}