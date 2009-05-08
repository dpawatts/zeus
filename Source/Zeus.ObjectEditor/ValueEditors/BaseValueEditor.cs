using System.Web.UI;

namespace Zeus.ObjectEditor.ValueEditors
{
	public abstract class BaseValueEditor : IValueEditor
	{
		/// <summary>
		/// Gets or sets a value which indicates whether this editor requires a value to be set.
		/// </summary>
		public bool Required { get; set; }

		/// <summary>
		/// Allowed the value editor to add its own validators. These validators are in addition
		/// to the generic ones added by BaseEditor.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="editor"></param>
		public abstract void AddValidators(Control panel, Control editor);

		public abstract Control CreateEditor();
		public abstract void DisableEditor(Control editor);
		public abstract object GetEditorValue(Control editor);
		public abstract void SetEditorValue(Control editor, object value);
	}
}