using System;
using System.Web.UI;
using Zeus.ContentProperties;
using Zeus.Design.Editors;
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

		public UploadMethod UploadMethod { get; set; }

		#endregion

		protected override Control CreateDetailEditor(int id, PropertyData detail)
		{
			ObjectProperty linkDetail = detail as ObjectProperty;

			FileDataEditor fileUpload = CreateEditor();
			fileUpload.ID = ID + "_upl_" + id;
			fileUpload.FileUploadImplementation = CreateUpload(fileUpload);

			if (linkDetail != null)
				fileUpload.CurrentFileName = ((FileData) linkDetail.Value).FileName;

			return fileUpload;
		}

		protected virtual FileDataEditor CreateEditor()
		{
			return new FileDataEditor();
		}

		private FileUploadImplementation CreateUpload(FileDataEditor fileUpload)
		{
			switch (UploadMethod)
			{
				case UploadMethod.Silverlight:
					return new SilverlightFileUploadImplementation(fileUpload);
				case UploadMethod.Flash:
					return new FlashFileUploadImplementation(fileUpload);
				default:
					throw new NotSupportedException();
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
			writer.Write("<br style=\"clear:both\" />");
		}
	}
}