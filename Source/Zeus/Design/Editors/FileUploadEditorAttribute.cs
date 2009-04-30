using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.Admin;
using Zeus.ContentTypes;
using Zeus.FileSystem;
using Zeus.Web.UI.WebControls;
using File = Zeus.FileSystem.File;

namespace Zeus.Design.Editors
{
	/// <summary>
	/// Attribute used to mark properties as editable. This attribute is predefined to use 
	/// the <see cref="System.Web.UI.WebControls.TextBox"/> web control as editor.</summary>
	/// <example>
	/// [N2.Details.EditableTextBox("Published", 80)]
	/// public override DateTime Published
	/// {
	///     get { return base.Published; } 
	///     set { base.Published = value; }
	/// }
	/// </example>
	[AttributeUsage(AttributeTargets.Property)]
	public class FileUploadEditorAttribute : AbstractEditorAttribute
	{
		/// <summary>Initializes a new instance of the EditableTextBoxAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public FileUploadEditorAttribute(string title, int sortOrder, string folderPath)
			: base(title, sortOrder)
		{
			FolderPath = folderPath;
		}

		#region Properties

		public string FolderPath { get; set; }

		#endregion

		protected virtual File CreateNewItem()
		{
			return new File();
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			FileUpload fileUpload = (FileUpload) editor;
			File existingImage = item[Name] as File;

			bool result = false;
			if (fileUpload.HasFile)
			{
				// Add new file.
				File newImage = existingImage;
				if (newImage == null)
				{
					newImage = CreateNewItem();

					Folder folder = (Folder) Context.Current.Resolve<Navigator>().Navigate(FolderPath);
					if (folder == null)
						throw new ZeusException("Folder path '{0}' does not exist", FolderPath);
					newImage.AddTo(folder);
				}

				newImage.Name = Path.GetFileName(fileUpload.PostedFile.FileName);
				newImage.Data = fileUpload.FileBytes;
				newImage.ContentType = fileUpload.PostedFile.ContentType;
				newImage.Size = fileUpload.PostedFile.ContentLength;

				Context.Persister.Save(newImage);

				item[Name] = newImage;

				result = true;
			}

			if (OnItemUpdated(item, editor))
				result = true;

			return result;
		}

		protected virtual bool OnItemUpdated(IEditableObject item, Control editor)
		{
			FileEditor fileUpload = editor as FileEditor;
			if (fileUpload.ShouldClear)
			{
				item[Name] = null;
				return true;
			}

			return false;
		}

		/// <summary>Creates a text box editor.</summary>
		/// <param name="container">The container control the tetx box will be placed in.</param>
		/// <returns>A text box control.</returns>
		protected override Control AddEditor(Control container)
		{
			Control fileUpload = CreateEditor();
			fileUpload.ID = Name;
			container.Controls.Add(fileUpload);

			return fileUpload;
		}

		protected override void DisableEditor(Control editor)
		{
			((FileEditor) editor).Enabled = false;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			File file = item[Name] as File;
			if (file != null)
			{
				FileEditor fileUpload = editor as FileEditor;
				fileUpload.ContentID = file.ID;
			}
		}

		protected virtual Control CreateEditor()
		{
			return new FileEditor();
		}
	}
}