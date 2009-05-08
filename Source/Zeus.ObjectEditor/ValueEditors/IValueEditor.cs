using System.Web.UI;

namespace Zeus.ObjectEditor.ValueEditors
{
	public interface IValueEditor
	{
		Control CreateEditor();
		void DisableEditor(Control editor);
		object GetEditorValue(Control editor);
		void SetEditorValue(Control editor, object value);

		/// <summary>
		/// Allowed the value editor to add its own validators. These validators are in addition
		/// to the generic ones added by BaseEditor.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="editor"></param>
		void AddValidators(Control panel, Control editor);
	}
}