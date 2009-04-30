using System.Web.UI;
using Zeus.ContentProperties;
using Zeus.FileSystem;

namespace Zeus.Web.UI.WebControls
{
	public class MultiFileDataUploadEditor : BaseDetailCollectionEditor
	{
		#region Properties

		protected override string Title
		{
			get { return "File"; }
		}

		#endregion

		protected override Control CreateDetailEditor(int id, PropertyData detail)
		{
			ObjectProperty linkDetail = detail as ObjectProperty;

			FileDataEditor fileUpload = CreateEditor();
			fileUpload.ID = ID + "_upl_" + id;

			if (linkDetail != null)
				fileUpload.CurrentFileName = ((FileData) linkDetail.Value).FileName;

			return fileUpload;
		}

		protected virtual FileDataEditor CreateEditor()
		{
			return new FileDataEditor();
		}
	}
}