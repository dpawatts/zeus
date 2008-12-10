using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using Zeus.FileSystem;
using Zeus.Web.UI.WebControls;

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
	public class ImageUploadEditorAttribute : AbstractEditorAttribute
	{
		/// <summary>Initializes a new instance of the EditableTextBoxAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public ImageUploadEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		#region Properties

		#endregion

		public override bool UpdateItem(ContentItem item, Control editor)
		{
			ImageEditor fileUpload = editor as ImageEditor;
			IFileSystem fileSystem = Zeus.Context.Current.Resolve<IFileSystem>();
			IFileIdentifier fileIdentifier = item[this.Name] as IFileIdentifier;

			if (fileUpload.HasFile)
			{
				if (fileIdentifier != null)
					fileSystem.DeleteFile(fileIdentifier);

				// Add new file.
				fileIdentifier = fileSystem.AddFile(fileUpload.FileName, fileUpload.FileBytes);
				item[this.Name] = fileIdentifier;
			}
			else if (fileUpload.ClearImage)
			{
				fileSystem.DeleteFile(fileIdentifier);
				item[this.Name] = null;
			}

			return true;
		}

		public override void UpdateEditor(ContentItem item, Control editor)
		{
			IFileIdentifier fileIdentifier = item[this.Name] as IFileIdentifier;
			if (fileIdentifier != null)
			{
				IFileSystem fileSystem = Zeus.Context.Current.Resolve<IFileSystem>();
				ImageEditor fileUpload = editor as ImageEditor;
				fileUpload.Image = fileSystem.GetFile(fileIdentifier).Data;
			}
		}

		/// <summary>Creates a text box editor.</summary>
		/// <param name="container">The container control the tetx box will be placed in.</param>
		/// <returns>A text box control.</returns>
		protected override Control AddEditor(Control container)
		{
			ImageEditor fileUpload = new ImageEditor();
			fileUpload.ID = Name;
			container.Controls.Add(fileUpload);

			return fileUpload;
		}
	}
}
