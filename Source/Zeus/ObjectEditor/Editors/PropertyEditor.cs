using System.Web.UI;
using Zeus.ObjectEditor.ValueEditors;

namespace Zeus.ObjectEditor.Editors
{
	public class PropertyEditor : BaseEditor
	{
		private readonly IValueEditor _valueEditor;

		public PropertyEditor(IValueEditor valueEditor)
		{
			_valueEditor = valueEditor;
		}

		protected override void DisableEditor(Control editor)
		{
			_valueEditor.DisableEditor(editor);
		}

		protected override Control AddEditor(Control panel)
		{
			Control editor = _valueEditor.CreateEditor();
			panel.Controls.Add(editor);
			_valueEditor.AddValidators(panel, editor);
			return editor;
		}

		public override bool UpdateObject(IEditableObject @object, Control editor)
		{
			object newValue = _valueEditor.GetEditorValue(editor);
			object currentValue = @object.GetPropertyValue(Name);
			if (!AreEqual(newValue, currentValue))
			{
				@object.SetPropertyValue(Name, newValue);
				return true;
			}
			return false;
		}

		protected override void UpdateEditorInternal(IEditableObject @object, Control editor)
		{
			object value = @object.GetPropertyValue(Name);
			_valueEditor.SetEditorValue(editor, value);
		}
	}
}