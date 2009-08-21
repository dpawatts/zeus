using Zeus.FileSystem;
using Zeus.FileSystem.Images;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	public class MultiImageUploadEditorAttribute : MultiFileUploadEditorAttribute
	{
		public MultiImageUploadEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override File CreateNewItem()
		{
			return new Image();
		}

		protected override BaseDetailCollectionEditor CreateEditor()
		{
			return new MultiImageUploadEditor();
		}
	}
}