using System;
using System.Web.UI;
using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.FileSystem;

namespace Zeus.Web.UI.WebControls
{
	public class MultiFileUploadEditor : BaseDetailCollectionEditor
	{
		#region Properties

		protected override string Title
		{
			get { return "File"; }
		}

		#endregion

		protected override Control CreateDetailEditor(int id, PropertyData detail)
		{
			/*ObjectProperty linkDetail = detail as ObjectProperty;

			FileDataEditor fileUpload = CreateEditor();
			fileUpload.ID = ID + "_upl_" + id;
			fileUpload.FileUploadImplementation = CreateUpload(fileUpload);

			if (linkDetail != null)
				fileUpload.CurrentFileName = ((File) linkDetail.Value).FileName;

			return fileUpload;*/
			throw new NotImplementedException();
		}

		protected virtual FancyFileUpload CreateEditor()
		{
			return new FancyFileUpload();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
			writer.Write("<br style=\"clear:both\" />");
		}
	}
}