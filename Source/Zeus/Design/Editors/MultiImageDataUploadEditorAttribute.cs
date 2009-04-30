using Zeus.FileSystem;
using Zeus.FileSystem.Images;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	public class MultiImageDataUploadEditorAttribute : MultiFileDataUploadEditorAttribute
	{
		public MultiImageDataUploadEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override FileData CreateNewItem()
		{
			return new ImageData();
		}

		protected override BaseDetailCollectionEditor CreateEditor()
		{
			return new MultiImageDataUploadEditor();
		}
	}
}