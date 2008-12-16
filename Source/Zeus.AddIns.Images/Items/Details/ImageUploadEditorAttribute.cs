using System;
using Zeus.ContentTypes.Properties;
using Zeus.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.AddIns.Images.UI.WebControls;

namespace Zeus.AddIns.Images.Items.Details
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

		public override void UpdateEditor(ContentItem item, Control editor)
		{
			Image file = item[this.Name] as Image;
			if (file != null)
			{
				ImageEditor fileUpload = editor as ImageEditor;
				fileUpload.ContentID = file.ID;
			}
		}

		public override bool UpdateItem(ContentItem item, Control editor)
		{
			bool result = base.UpdateItem(item, editor);

			ImageEditor fileUpload = editor as ImageEditor;
			if (fileUpload.ClearImage)
			{
				item[this.Name] = null;
				result = true;
			}

			return result;
		}

		protected override System.Web.UI.Control CreateEditor()
		{
			return new ImageEditor();
		}
	}
}
