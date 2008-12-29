using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using Zeus.FileSystem;
using Zeus.Web.UI.WebControls;
using Zeus.Admin;
using Zeus.Web.UI;

namespace Zeus.ContentTypes.Properties
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
			this.FolderPath = folderPath;
		}

		#region Properties

		public string FolderPath
		{
			get;
			set;
		}

		#endregion

		protected virtual File CreateNewItem()
		{
			return new File();
		}

		public override bool UpdateItem(ContentItem item, Control editor)
		{
			FileUpload fileUpload = editor as FileUpload;
			File existingImage = item[this.Name] as File;

			bool result = false;
			if (fileUpload.HasFile)
			{
				// Add new file.
				File newImage = existingImage;
				if (newImage == null)
				{
					newImage = CreateNewItem();

					Folder folder = (Folder) Zeus.Context.Current.Resolve<Navigator>().Navigate(this.FolderPath);
					newImage.AddTo(folder);
				}

				newImage.Name = System.IO.Path.GetFileName(fileUpload.PostedFile.FileName);
				newImage.Data = fileUpload.FileBytes;
				newImage.ContentType = fileUpload.PostedFile.ContentType;
				newImage.Size = fileUpload.PostedFile.ContentLength;

				item[this.Name] = newImage;

				result = true;
			}

			if (OnItemUpdated(item, editor))
				result = true;

			return result;
		}

		protected virtual bool OnItemUpdated(ContentItem item, Control editor)
		{
			FileEditor fileUpload = editor as FileEditor;
			if (fileUpload.ShouldClear)
			{
				item[this.Name] = null;
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

		public override void UpdateEditor(ContentItem item, Control editor)
		{
			File file = item[this.Name] as File;
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
