using System;
using Zeus.ContentTypes;
using Zeus.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using Image=Zeus.FileSystem.Images.Image;

namespace Zeus.Design.Editors
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ImageUploadEditorAttribute : FileUploadEditorAttribute
	{
		/// <summary>Initializes a new instance of the EditableTextBoxAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public ImageUploadEditorAttribute(string title, int sortOrder, string folderPath)
			: base(title, sortOrder, folderPath)
		{

		}

		protected override Zeus.FileSystem.File CreateNewItem()
		{
			return new Image();
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			Image file = item[this.Name] as Image;
			if (file != null)
			{
				ImageEditor fileUpload = editor as ImageEditor;
				fileUpload.ContentID = file.ID;
			}
		}

		protected override bool OnItemUpdated(IEditableObject item, Control editor)
		{
			ImageEditor fileUpload = editor as ImageEditor;
			if (fileUpload.ClearImage)
			{
				item[this.Name] = null;
				return true;
			}

			return false;
		}

		protected override System.Web.UI.Control CreateEditor()
		{
			return new ImageEditor();
		}
	}
}