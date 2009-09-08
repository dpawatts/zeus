using System;
using System.IO;
using System.Web.UI;
using Isis.ExtensionMethods.IO;
using Isis.Web;
using Zeus.ContentProperties;
using Zeus.FileSystem;
using Zeus.Web.Handlers;
using Zeus.Web.UI.WebControls;
using File=Zeus.FileSystem.File;

namespace Zeus.Design.Editors
{
	public class MultiFileUploadEditorAttribute : BaseDetailCollectionEditorAttribute
	{
		public MultiFileUploadEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override void CreateOrUpdateDetailCollectionItem(ContentItem contentItem, PropertyData existingDetail, Control editor, out object newDetail)
		{
			FancyFileUpload fileEditor = (FancyFileUpload)editor;
			LinkProperty existingFileProperty = existingDetail as LinkProperty;
			if (fileEditor.HasNewOrChangedFile)
			{
				// Add new file.
				File newFile = null;
				if (existingFileProperty != null)
					newFile = existingFileProperty.LinkedItem as File;
				if (newFile == null)
				{
					newFile = CreateNewItem();
					newFile.Name = Name + Guid.NewGuid();
					newFile.AddTo(contentItem);
				}

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

		protected virtual File CreateNewItem()
		{
			return new File();
		}

		protected override BaseDetailCollectionEditor CreateEditor()
		{
			return new MultiFileUploadEditor();
		}
	}
}