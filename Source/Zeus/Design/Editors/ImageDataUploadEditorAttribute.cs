using System;
using Zeus.ContentTypes;
using Zeus.FileSystem;
using Zeus.FileSystem.Images;
using Zeus.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ImageDataUploadEditorAttribute : FileDataUploadEditorAttribute
	{
		/// <summary>Initializes a new instance of the EditableTextBoxAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public ImageDataUploadEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override FileData CreateNewItem()
		{
			return new ImageData();
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			base.UpdateEditorInternal(item, editor);
			/*Image file = item[this.Name] as Image;
			if (file != null)
			{
				ImageEditor fileUpload = editor as ImageEditor;
				fileUpload.ContentID = file.ID;
			}*/
		}

		protected override bool OnItemUpdated(IEditableObject item, Control editor)
		{
			/*ImageEditor fileUpload = editor as ImageEditor;
			if (fileUpload.ClearImage)
			{
				item[this.Name] = null;
				return true;
			}

			*/return false;
		}

		protected override FileDataEditor CreateEditor()
		{
			return new ImageDataEditor();
		}
	}
}