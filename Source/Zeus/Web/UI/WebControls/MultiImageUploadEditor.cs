namespace Zeus.Web.UI.WebControls
{
	public class MultiImageUploadEditor : MultiFileUploadEditor
	{
		#region Properties

		protected override string Title
		{
			get { return "Image"; }
		}

		#endregion

		protected override FancyFileUpload CreateEditor()
		{
			return new FancyImageUpload();
		}
	}
}