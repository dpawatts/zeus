using System;
using System.IO;
using System.Web;
using System.Web.UI;
using Zeus.BaseLibrary.ExtensionMethods.IO;
using Zeus.BaseLibrary.Web;
using Zeus.ContentTypes;
using Zeus.Web.Handlers;
using Zeus.Web.UI.WebControls;
using File = Zeus.FileSystem.File;

namespace Zeus.Design.Editors
{
	[AttributeUsage(AttributeTargets.Property)]
	public class FileUploadEditorAttribute : AbstractEditorAttribute
	{
		/// <summary>Initializes a new instance of the EditableTextBoxAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public FileUploadEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{

		}

		#region Properties

		public string TypeFilterDescription { get; set; }
		public string[] TypeFilter { get; set; }
		public int MaximumFileSize { get; set; }

		#endregion

		public static string GetUploadedFilePath(FancyFileUpload fileUpload)
		{
			string uploadFolder = BaseFileUploadHandler.GetUploadFolder(fileUpload.Identifier);
			return Path.Combine(uploadFolder, HttpUtility.UrlDecode(fileUpload.FileName));
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			FancyFileUpload fileUpload = (FancyFileUpload) editor;
			File file = (File) item;

			bool result = false;
			if (fileUpload.HasDeletedFile)
			{
				file.Data = null;
				result = true;
			}
			else if (fileUpload.HasNewOrChangedFile)
			{
				// Populate File object.
				file.FileName = fileUpload.FileName;
				string uploadedFile = GetUploadedFilePath(fileUpload);
				using (FileStream fs = new FileStream(uploadedFile, FileMode.Open))
				{
					file.Data = fs.ReadAllBytes();
					file.ContentType = MimeUtility.GetMimeType(file.Data);
					file.Size = fs.Length;
				}

				// Delete temp folder.
				System.IO.File.Delete(uploadedFile);
				Directory.Delete(BaseFileUploadHandler.GetUploadFolder(fileUpload.Identifier));

				result = true;
			}

			return result;
		}

		/// <summary>Creates a text box editor.</summary>
		/// <param name="container">The container control the tetx box will be placed in.</param>
		/// <returns>A text box control.</returns>
		protected override Control AddEditor(Control container)
		{
			FancyFileUpload fileUpload = CreateEditor();
			fileUpload.ID = Name;
			if (!string.IsNullOrEmpty(TypeFilterDescription))
				fileUpload.TypeFilterDescription = TypeFilterDescription;
			if (TypeFilter != null)
				fileUpload.TypeFilter = TypeFilter;
			if (MaximumFileSize > 0)
				fileUpload.MaximumFileSize = MaximumFileSize;
			container.Controls.Add(fileUpload);

			return fileUpload;
		}

		protected override void DisableEditor(Control editor)
		{
			((FancyFileUpload)editor).Enabled = false;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			FancyFileUpload fileUpload = (FancyFileUpload)editor;
			File file = (File)item;
			if (!file.IsEmpty())
				fileUpload.CurrentFileName = file.FileName;
		}

		protected virtual FancyFileUpload CreateEditor()
		{
			return new FancyFileUpload();
		}
	}
}