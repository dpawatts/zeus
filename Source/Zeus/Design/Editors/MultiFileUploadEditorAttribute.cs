using System.IO;
using System.Web.UI;
using Isis.ExtensionMethods.IO;
using Isis.Web;
using Zeus.FileSystem;
using Zeus.Web.Handlers;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	public class MultiFileDataUploadEditorAttribute : BaseDetailCollectionEditorAttribute
	{
		public MultiFileDataUploadEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override void CreateOrUpdateDetailCollectionItem(object existingDetail, Control editor, out object newDetail)
		{
			FileDataEditor fileEditor = (FileDataEditor) editor;
			FileData existingFile = existingDetail as FileData;
			if (fileEditor.HasNewOrChangedFile)
			{
				// Add new file.
				FileData newFile = existingFile ?? CreateNewItem();

				// Populate FileData object.
				newFile.FileName = fileEditor.FileName;
				string uploadFolder = BaseFileUploadHandler.GetUploadFolder(fileEditor.Identifier);
				string uploadedFile = Path.Combine(uploadFolder, fileEditor.FileName);
				using (FileStream fs = new FileStream(uploadedFile, FileMode.Open))
				{
					newFile.Data = fs.ReadAllBytes();
					newFile.ContentType = MimeUtility.GetMimeType(newFile.Data);
					newFile.Size = fs.Length;
				}

				// Delete temp folder.
				System.IO.File.Delete(uploadedFile);
				Directory.Delete(uploadFolder);

				newDetail = newFile;
			}
			else
			{
				newDetail = null;
			}
		}

		protected virtual FileData CreateNewItem()
		{
			return new FileData();
		}

		protected override BaseDetailCollectionEditor CreateEditor()
		{
			return new MultiFileDataUploadEditor();
		}
	}
}