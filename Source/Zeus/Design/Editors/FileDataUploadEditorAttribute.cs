using System;
using System.IO;
using System.Web.UI;
using Isis.ExtensionMethods.IO;
using Isis.Web;
using Zeus.ContentTypes;
using Zeus.FileSystem;
using Zeus.Web.Handlers;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	[AttributeUsage(AttributeTargets.Property)]
	public class FileDataUploadEditorAttribute : AbstractEditorAttribute
	{
		/// <summary>Initializes a new instance of the FileUploadEditorAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public FileDataUploadEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		public FileDataUploadEditorAttribute()
		{
			
		}

		protected virtual FileData CreateNewItem()
		{
			return new FileData();
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			FancyFileUpload fileEditor = (FancyFileUpload)editor;
			FileData existingFile = item[Name] as FileData;

			bool result = false;
			if (fileEditor.HasDeletedFile)
			{
				item[Name] = null;
				result = true;
			}
			else if (fileEditor.HasNewOrChangedFile)
			{
				// Add new file.
				FileData newFile = existingFile ?? CreateNewItem();

				// Populate FileData object.
				newFile.FileName = fileEditor.FileName;
				string uploadFolder = BaseFileUploadHandler.GetUploadFolder(fileEditor.Identifier);
				string uploadedFile = Path.Combine(uploadFolder, fileEditor.Page.Server.UrlDecode(fileEditor.FileName));
				using (FileStream fs = new FileStream(uploadedFile, FileMode.Open))
				{
					newFile.Data = fs.ReadAllBytes();
					newFile.ContentType = MimeUtility.GetMimeType(newFile.Data);
					newFile.Size = fs.Length;
				}

				// Delete temp folder.
				System.IO.File.Delete(uploadedFile);
				Directory.Delete(uploadFolder);

				item[Name] = newFile;

				result = true;
			}

			if (OnItemUpdated(item, editor))
				result = true;

			return result;
		}

		protected virtual bool OnItemUpdated(IEditableObject item, Control editor)
		{
			/*FileEditor fileUpload = (FileEditor) editor;
			if (fileUpload.ShouldClear)
			{
				item[Name] = null;
				return true;
			}
			*/
			return false;
		}

		/// <summary>Creates a text box editor.</summary>
		/// <param name="container">The container control the tetx box will be placed in.</param>
		/// <returns>A text box control.</returns>
		protected override Control AddEditor(Control container)
		{
			FancyFileUpload fileUpload = CreateEditor();
			fileUpload.ID = Name;
			container.Controls.Add(fileUpload);

			return fileUpload;
		}

		protected override void DisableEditor(Control editor)
		{
			((FancyFileUpload)editor).Enabled = false;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			FileData file = item[Name] as FileData;
			if (file != null)
			{
				FancyFileUpload fileUpload = (FancyFileUpload)editor;
				fileUpload.CurrentFileName = file.FileName;
			}
		}

		protected virtual FancyFileUpload CreateEditor()
		{
			return new FancyFileUpload();
		}
	}
}