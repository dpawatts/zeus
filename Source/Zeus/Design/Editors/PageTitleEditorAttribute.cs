namespace Zeus.Design.Editors
{
	public class PageTitleEditorAttribute : ReactiveTextBoxEditorAttribute
	{
		public PageTitleEditorAttribute()
			: base("{Title}")
		{
			MaxLength = 200;
		}

		protected override void UpdateEditorInternal(Zeus.ContentTypes.IEditableObject item, System.Web.UI.Control editor)
		{
			if (!Context.ContentTypes.GetContentType(item.GetType()).IsPage)
				editor.Parent.Visible = false;

			base.UpdateEditorInternal(item, editor);
		}
	}
}