namespace Zeus.Web.UI.WebControls
{
	public class MultiImageDataUploadEditor : MultiFileDataUploadEditor
	{
		#region Properties

		protected override string Title
		{
			get { return "Image"; }
		}

		#endregion

		protected override FileDataEditor CreateEditor()
		{
			return new ImageDataEditor();
		}
	}
}